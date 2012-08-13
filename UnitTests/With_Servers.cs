using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;

using SvcRestart;

using log4net;

namespace UnitTests
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class With_Servers
    {
        private static ILog log;

        [SetUp]
        public void Should_set_up()
        {
            log = LogManager.GetLogger("log");
            log4net.Config.XmlConfigurator.Configure();
        }

        [Test]
        public void Should_Get_Server__list()
        {
            var servers = new Servers("LDAP://INT.Bti.local", log);
            var list = servers.Get();
            Parallel.ForEach(list, name =>
                {
                    Console.WriteLine(name);
                    Console.WriteLine("Processing {0} on thread {1}", name, Thread.CurrentThread.ManagedThreadId);

                } //close lambda expression
            ); //close method invocation
        }


        [Test]
        public void Should_restart_service()
        {
            var svcState = new SvcState("CruiseAgent55", "SQLWriter", log);
            svcState.RestartService(10000);
        }
    }
}
