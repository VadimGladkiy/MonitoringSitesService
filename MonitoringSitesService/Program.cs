using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace MonitoringSitesService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly string host_address = "http://localhost:888/";
        static void Main()
        {
            #if DEBUG
            System.Diagnostics.Debugger.Launch();
            string comands = "c: \n cd c:\\Windows\\System32 \n net start service1 \n";
            string[] lines = comands.Split('\n');
            Process myProcess = new Process();
            ProcessStartInfo startInfo =
                new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Verb = "open";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            // start cmd
            //appoint startInfo for process
            myProcess.StartInfo = startInfo;
            myProcess.Start();
            //
            StreamWriter cmdWriter = myProcess.StandardInput;
            if (cmdWriter.BaseStream.CanWrite)
            {
                foreach(var line in lines)
                cmdWriter.WriteLine(line);
            }
            
            cmdWriter.Close();
        #endif


            // Create an Instance of ServiceController
            ServiceController myService = new ServiceController();
            // Define the name of your service here. 
            // I am using the 'Service1' for this example
           
            // After this point, myService is now refering to "MonitoringSitesService"
            myService.ServiceName = "Service1";
            
            // array of services 
            ServiceBase[] ServicesToRun;
            
                using (WebApp.Start<Startup>(url: host_address))
                {
                    ServicesToRun = new ServiceBase[]
                    {
                        new Service1()
                    };
                    var controller = new StatusController();
                    ServiceBase.Run(ServicesToRun);
                }
            
            
            // Get the status of myService
            // Possible Status Returns: { StartPending, Running, PausePending, Paused, 
            // StopPending, Stopped, ContinuePending }

            
        }
    }
}
