using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Agent.Console {
    public class Service  {
        public Service()
        {
        }
        public void Start(string[] args)
        {
            System.Console.WriteLine(args);

            System.Console.ReadKey();
        }

        public void Stop()
        {
            System.Console.WriteLine("Stopping");
        }
    }
}
