using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAgentDataTool.Helpers
{
    public class Singleton
    {
        private static readonly Singleton instance = new Singleton();

        private Singleton()
        {
            // These things must only happen once.
        }    
        public static Singleton Instance { get { return instance; } }

        public void DoSomething()
        {
            // This must be thread-safe!
        }
    }
}
