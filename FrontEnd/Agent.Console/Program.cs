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
            if (!Environment.UserInteractive)
            {

            }
            System.Console.WriteLine("Running as a Console Application");
            System.Console.WriteLine(" 1. Run Service");
            System.Console.WriteLine(" 2. Other Option");
            System.Console.WriteLine(" 3. Exit");
            System.Console.Write("Enter Option: ");

            var input = System.Console.ReadLine();

            switch (input)
            {
                case "1":
                    HostFactory.Run(topShelf =>
                    {
                        topShelf.Service<Service>(service =>
                        {
                            service.ConstructUsing(UriHostNameType => new Service());
                            service.WhenStarted(svc => svc.Start(args));
                            service.WhenStopped(svc => svc.Stop());
                        });
                        topShelf.UseSerilog();
                        topShelf.RunAsLocalSystem();

                        topShelf.SetDescription("Agent client host");
                        topShelf.SetDisplayName("Kit McCloud");
                        topShelf.SetServiceName("Smart Agent Data Service");
                    });
                    break;
                case "2":
                    break;
                default:
                    break;
            }
            //HostFactory.Run(topShelf =>
            //{
            //    topShelf.Service<Service>(service =>
            //    {
            //        service.ConstructUsing(UriHostNameType => new Service());
            //        service.WhenStarted(svc => svc.Start(args));
            //        service.WhenStopped(svc => svc.Stop());
            //    });
            //    topShelf.UseSerilog();
            //    topShelf.RunAsLocalSystem();
            //    topShelf.SetDescription("Agent client host");
            //    topShelf.SetDisplayName("Kit McCloud");
            //    topShelf.SetServiceName("Smart Agent Data Service");
            //});

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
