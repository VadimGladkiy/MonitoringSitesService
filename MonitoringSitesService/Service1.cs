using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading.Tasks;
using AskSites;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.Threading;


namespace MonitoringSitesService
{
    public partial class Service1 : ServiceBase
    {
        private AskSites.EventsManager _provider;
        private Thread generalThread;
        private readonly Thread myThread;
      //private Thread webAppThread;
        

        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;

            // set status of service
            StatusController.setServiceStatus(true);
            
            // create provider
            _provider = new EventsManager();
            _provider.RisingTimer += _provider_RisingTimer;

            // create threads
            myThread = new Thread(IsWork);
            myThread.SetApartmentState(ApartmentState.STA);

            generalThread = new Thread(_provider.RunEventsManager);
            generalThread.SetApartmentState(ApartmentState.STA);

            /*webAppThread = new Thread(WebApp.Start<Startup>(url: host_address));
            webAppThread.SetApartmentState(ApartmentState.STA);*/
        }

        private void _provider_RisingTimer()
        {
            SendRequest(_provider.getSiteName());
        }

        protected override void OnStart(string[] args)
        {

            myThread.Start();
            generalThread.Start();
          //  string address = "http://localhost:9000/";
          //  run host
          //  using (WebApp.Start<Startup>(url: address))
          //  { }
            
            StatusController.setServiceStatus(true);
            AddLog("start");
        }
        protected override void OnStop()
        {
          //  _provider.StopEventsManager();
            myThread.Abort();
            generalThread.Abort();
            _provider.StopEventsManager();
            Thread.Sleep(1000);
            StatusController.setServiceStatus(false);
            AddLog("stop");
        }
        protected override void OnPause()
        {
            _provider.StopEventsManager();
            StatusController.setServiceStatus(false);
            AddLog("stop");
        }
        protected override void OnContinue()
        {
            _provider.RunEventsManager();
            StatusController.setServiceStatus(true);
            AddLog("start");
        }
        public void AddLog(string log)
        {
            try
            {
                if (!EventLog.SourceExists("MyExampleService"))
                {
                    EventLog.CreateEventSource("MyExampleService", "MyExampleService");
                }
                eventLog1.Source = "MyExampleService";
                eventLog1.WriteEntry(log);
            }
            catch { }
        }
        public void SendRequest(string arg)
        {
            // create the client and do request 
            HttpClient client = new HttpClient();
            string response; 
            try
            {
                response = client.GetAsync(arg).Result.StatusCode.ToString();
            }
            catch (Exception e) { response = e.Message; }

            _provider.SaveResponse(response);
        }
        private void IsWork()
        {
            string str = "it is working";
        }
    }
}
