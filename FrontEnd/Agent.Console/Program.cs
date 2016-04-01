using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
using AgentDataServices;


namespace Agent.Console {
    static class Program {
        public class TownCrier {
            readonly Timer _timer;
            public TownCrier()
            {
                _timer = new Timer(1000) { AutoReset = true };
                _timer.Elapsed += (sender, eventArgs) => System.Console.WriteLine("It is {0} and all is well", DateTime.Now);
            }
            public void Start()
            {
                _timer.Start();
            }
            public void Stop()
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(params string[] args)
        {
            System.Console.WriteLine("Running as a Console Application");
            System.Console.WriteLine(" 1. Create Criteria Record");
            System.Console.WriteLine(" 2. Other Option");
            System.Console.WriteLine(" 3. Exit");
            System.Console.Write("Enter Option: ");
            var input = System.Console.ReadLine();
          
            switch (input)
            {
                case "1":
                    RunCriteriaCreator(args);
                    break;
                case "2":
                    break;
                default:
                    break;
            }
        }

        static void RunCriteriaCreator(string[] args)
        {
            System.Console.WriteLine("Pleasee Enter a criteria set name");
            var criteriaSetname = System.Console.ReadLine();

            System.Console.WriteLine("Enter a inital scriptkey");
            var initalScriptKey = System.Console.ReadLine();

            System.Console.WriteLine("Enter a iprkey");
            var iprkey = System.Console.ReadLine();

            System.Console.WriteLine("Enter client key");
            string clientKey = System.Console.ReadLine();
            var toGuid = new Guid(clientKey);

            System.Console.WriteLine("Enter client key");


            System.Console.WriteLine(criteriaSetname);
            HostFactory.Run(topShelf =>
            {
                topShelf.SetDescription("Agent client host");
                topShelf.SetDisplayName("Kit McCloud");
                topShelf.SetServiceName("Smart Agent Data Service");
                topShelf.UseSerilog();

                topShelf.RunAsLocalSystem();
                topShelf.Service<Service>(service =>
                {
                    service.ConstructUsing(UriHostNameType => new Service());
                    service.WhenStarted(svc => svc.Start(args));
                    service.WhenStopped(svc => svc.Stop());
                });
            });
        }
        static void ConfigureSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile(@"timerlogs\$safeprojectname$-{Date}.txt")
                .CreateLogger()
                .ForContext("ApplicationName", "");
        }
    }
}
