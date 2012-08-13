using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;

namespace SvcRestart
{
    class Program
    {
        private static ILog log;

        static void Main(string[] args)
        {            
            try
            {
                if (args.Count() < 3 || args.Count() > 4)
                {
                    Console.WriteLine("HELP: SvcRestart.exe [ldap] [service name] [Timeout in seconds] [optional computerName contrains]");
                }
                else
                {
                    log = LogManager.GetLogger("log");
                    log4net.Config.XmlConfigurator.Configure();
                    var conputernamecontains = "";
                    var ldap = args[0];
                    var serviceName = args[1];
                    var timeoutSeconds = Convert.ToInt32(args[2]);
                    var application = new Application(ldap, log);
                    if (args.Count() == 4)
                    {
                        conputernamecontains = args[3];
                    }
                    application.Run(conputernamecontains, serviceName, timeoutSeconds);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
