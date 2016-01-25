using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using SmartAgent.Web.App_Start;
using Ninject;

namespace WebTool.Tests
{
    [TestClass]
    public class ServeStaticFilesTest
    {
        [TestMethod]
        public void GetStaticFilesTest() {
            var config = new WebApiConfig();
            IAppBuilder app = null;
            SmartAgent.Web.App_Start.WebApiConfig.Configure(app);            
        }
    }
}
