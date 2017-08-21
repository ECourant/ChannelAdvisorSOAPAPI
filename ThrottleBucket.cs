using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace ChannelAdvisorSOAP
{
    internal class ThrottleBucket : AErrorChild
    {
        [JsonIgnore]
        private static log4net.ILog LogNet = log4net.LogManager.GetLogger(typeof(ThrottleBucket));


        

        internal ThrottleBucket(string FunctionName)
        {
            this.FunctionName = FunctionName;
            this.Update();
        }
        internal ThrottleBucket(int MaxRequestsPerHour, string FunctionName)
        {
            this.FunctionName = FunctionName;
            this.Update();
            this.MaxRequests = MaxRequestsPerHour;
        }


        [JsonProperty("function_name")]
        private string FunctionName { get; set; }
        [JsonProperty("last_reset")]
        private DateTime LastReset { get; set; }
        [JsonProperty("next_reset")]
        private DateTime NextReset { get; set; }
        [JsonProperty("max_requests")]
        internal int MaxRequests = 50000;
        [JsonProperty("requests_made")]
        internal int RequestsMade = 0;

        private void Update()
        {
            if (DateTime.Now >= NextReset)
            {
                DateTime NewLastReset = DateTime.Now.AddMinutes(-DateTime.Now.Minute);
                if (LastReset != default(DateTime))
                    LogNet.Debug($"[{FunctionName}] Updating LastReset... CURRENT: [{LastReset.ToShortTimeString()}] | NEW: [{NewLastReset.ToShortTimeString()}]");
                else
                    LogNet.Debug($"[{FunctionName}] Updating LastReset... CURRENT: [N/A] | NEW: [{NewLastReset.ToShortTimeString()}]");
                LastReset = NewLastReset;
                DateTime NewNextReset = DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute);
                if (NextReset != default(DateTime))
                    LogNet.Debug($"[{FunctionName}] Updating NextReset... CURRENT: [{NextReset.ToShortTimeString()}] | NEW: [{NewNextReset.ToShortTimeString()}]");
                else
                    LogNet.Debug($"[{FunctionName}] Updating NextReset... CURRENT: [N/A] | NEW: [{NewNextReset.ToShortTimeString()}]");
                NextReset = NewNextReset;
                RequestsMade = 0;
            }
        }

        internal void Throttle()
        {
            try
            {
                this.Update();
                if (RequestsMade >= MaxRequests)
                {
                    LogNet.Warn($"[{FunctionName}] Request is being throttled, the request will be sent at: [{NextReset.ToString()}]!");
                    Task Wait = new Task(() =>
                    {
                        while (DateTime.Now < NextReset)
                            System.Threading.Thread.Sleep(1000);
                    });
                    Wait.Start();
                    if (Wait.Wait(TimeSpan.FromHours(1)))
                    {
                        LogNet.Debug($"[{FunctionName}] Sending Request [{(RequestsMade + 1)}/{MaxRequests}]");
                        RequestsMade++;
                    }
                    else
                        throw new TimeoutException("The request has been throttled for more than 1 hour and has timed out; The request will not be sent!");
                }
                else
                {
                    LogNet.Debug($"[{FunctionName}] Sending Request [{(RequestsMade + 1)}/{MaxRequests}]");
                    RequestsMade++;
                }
            }
            catch(Exception e)
            {
                LogNet.Error($"[{FunctionName}] An exception has been thrown while throttling request for function.", e);
                Report(e);
                throw e;
            }
        }
    }
}
