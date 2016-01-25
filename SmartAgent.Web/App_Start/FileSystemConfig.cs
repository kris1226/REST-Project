using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;


namespace SmartAgent.Web.App_Start {
    public class FileSystemConfig {

        public static void Configure(IAppBuilder app) {
            
            var contentDir = ConfigurationManager.AppSettings["smartAgentTool.rootDir"];
            var physicalFileSystem = new PhysicalFileSystem(contentDir);

            var options = new FileServerOptions {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };

            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.DefaultFilesOptions.DefaultFileNames = new[] { "index.html" };
            app.UseFileServer(options); 
        }
    }
}