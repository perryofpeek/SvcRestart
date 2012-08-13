using System;
using System.Threading;
using System.Threading.Tasks;

using log4net;

namespace SvcRestart
{
    public class Application
    {
        private readonly string ldap;

        private readonly ILog log;

        public Application(string ldap, ILog log)
        {
            this.ldap = ldap;
            this.log = log;
        }

        public void Run(string computerNamesContains,string serviceName,int timeoutSeconds)
        {
            var servers = new Servers(ldap, log);
            var list = servers.Get();
            Parallel.ForEach(list, name =>
            {
                LogicalThreadContext.Properties["MachineName"] = name;
                
                if(name.ToLower().Contains(computerNamesContains.ToLower()))
                {
                    log.Info("Processing");
                    var svcState = new SvcState(name, serviceName, log);
                    svcState.RestartService(timeoutSeconds * 1000);
                }
                
                //Console.WriteLine("Processing {0} on thread {1}", name, Thread.CurrentThread.ManagedThreadId);

            } //close lambda expression
            ); //close method invocation
        }
    }
}