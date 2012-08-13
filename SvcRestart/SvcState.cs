using System;
using System.ServiceProcess;

using log4net;

namespace SvcRestart
{
    public class SvcState
    {
        private readonly string machineName;

        private readonly string serviceName;

        private readonly ILog log;

        public SvcState(string machineName, string serviceName, ILog log)
        {
            this.machineName = machineName;
            this.serviceName = serviceName;
            this.log = log;           
        }

        public void RestartService(int timeoutMilliseconds)
        {
            log.Info(string.Format("Restarting {0}", serviceName));
            var service = new ServiceController(serviceName, machineName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                log.Debug(string.Format("Stopped {0}", serviceName));
                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                log.Debug(string.Format("Started  {0}", serviceName));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void StartService(int timeoutMilliseconds)
        {
            log.Info(string.Format("Starting  {0}", serviceName));
            var service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void StopService(int timeoutMilliseconds)
        {
            log.Info(string.Format("Stopping  {0}", serviceName));
            var service = new ServiceController(serviceName);
            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}