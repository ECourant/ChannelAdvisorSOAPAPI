using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
namespace ChannelAdvisorSOAP
{
    public sealed class ChannelAdvisorSOAPConnection : IDisposable
    {
        private static log4net.ILog LogNet = log4net.LogManager.GetLogger(typeof(ChannelAdvisorSOAPConnection));

        private Dictionary<string, ThrottleBucket> Timings { get; set; }
        private string Path = $"{System.Windows.Forms.Application.CommonAppDataPath}\\Throttle.json";
        private string LogPag = $"{System.Windows.Forms.Application.CommonAppDataPath}\\Throttle.log";

        private UnauthorizedAccessException UnauthorizedAccess = new UnauthorizedAccessException("You are not authenticated and cannot make function calls!");
        
        private Pipelines.AdminPipeline _Admin { get; set; }
        private Pipelines.InventoryPipeline _Inventory { get; set; }
        private Pipelines.OrdersPipeline _Orders { get; set; }
        private Pipelines.FulfillmentPipeline _Fulfillment { get; set; }
        //private System.Threading.Thread SaveTimingsThread { get; set; }


        private string DeveloperKey { get; set; }
        private string Password { get; set; }

        public ChannelAdvisorSOAPConnection(string DeveloperKey, string Password)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\My Project-c1a8ab96b9c2.json");
            this.DeveloperKey = DeveloperKey;
            this.Password = Password;
            Init();
            LogNet.Info("Started!");
        }




        public Pipelines.AdminPipeline Admin => this._Admin ?? throw this.UnauthorizedAccess;
        public Pipelines.InventoryPipeline Inventory => this._Inventory ?? throw this.UnauthorizedAccess;
        public Pipelines.OrdersPipeline Orders => this._Orders ?? throw this.UnauthorizedAccess;
        public Pipelines.FulfillmentPipeline Fulfillment => this._Fulfillment ?? throw this.UnauthorizedAccess;


        private void ThrottleSOAP(string Function)
        {
            if (!this.Timings.ContainsKey(Function))
                this.Timings.Add(Function, new ThrottleBucket(Function));
            this.Timings[Function].Throttle();
            SaveTimings();
        }
        private dynamic GetAPICredentials(Type CredentialsType)
        {
            dynamic Credentials = Activator.CreateInstance(CredentialsType);
            Credentials.DeveloperKey = this.DeveloperKey;
            Credentials.Password = this.Password;
            return Credentials;
        }

        private void Init()
        {
            try
            {
                this.Timings = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, ThrottleBucket>>(System.IO.File.ReadAllText(Path));
                foreach (var Key in ThrottleDefinitions.Timings.Where(Key => !this.Timings.ContainsKey(Key.Key)))
                    this.Timings.Add(Key.Key, Key.Value);
            }
            catch(Exception e)
            {
                this.Timings = ThrottleDefinitions.Timings;
                
            }
            //this.SaveTimingsThread = new System.Threading.Thread(() => this.SaveTimingsThreadFunction());
            //this.SaveTimingsThread.IsBackground = true;
            //this.SaveTimingsThread.Priority = System.Threading.ThreadPriority.Lowest;
            //this.SaveTimingsThread.Start();
            this._Admin = new Pipelines.AdminPipeline(this.ThrottleSOAP, this.GetAPICredentials);
            this._Inventory = new Pipelines.InventoryPipeline(this.ThrottleSOAP, this.GetAPICredentials);
            this._Orders = new Pipelines.OrdersPipeline(this.ThrottleSOAP, this.GetAPICredentials);
            this._Fulfillment = new Pipelines.FulfillmentPipeline(this.ThrottleSOAP, this.GetAPICredentials);
        }

        private void SaveTimings()
        {
            System.IO.File.WriteAllText(Path, Newtonsoft.Json.JsonConvert.SerializeObject(this.Timings, Newtonsoft.Json.Formatting.Indented));
        }
        private void SaveTimingsThreadFunction()
        {
            while (true)
            {
                DateTime NextSave = DateTime.Now.AddMinutes(1);
                while (DateTime.Now <= NextSave)
                    System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Saving Timings!");
                SaveTimings();
            }
        }




        #region Dispose Code Handles And Functions
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        internal void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                SaveTimings();
                //try
                //{
                //    this.SaveTimingsThread.Abort();
                //}
                //catch(Exception e)
                //{

                //}
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }
        #endregion
    }
}
