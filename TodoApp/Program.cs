using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.SelfHost;
using TodoApp.App_Start;
using Ninject.Web.Common.SelfHost;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;


namespace TodoApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            string uri = @"http://localhost:8080";

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Server started...");
                Console.ReadKey();
                Console.WriteLine("Server stopped!");
            }
        }
    }
}