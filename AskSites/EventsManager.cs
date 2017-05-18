using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Net.Http;
using System.Net;


namespace AskSites
{
    public class EventsManager
    {
        // Microsoft  // 1 time in 2 days
        // Google;    // 2 min
        // Apple;     // 5 min
        // Timers
        private Timer FirstWatcher;
        private Timer SecondWatcher;
        private Timer ThirdWatcher;
        //
        private static DateTime differ;
        private static bool FirstTime = false; 
        private static string siteName;
        
        private static DateTime GetTimeInfoFromFile()
        {
            // path to file input data
            string path = @"C:\MonitoringSitesServiceTemp\NextQueriesTimetable.txt";
            // variable to result
            DateTime Result;
            Stream myStream= null;
            string line = null;
            // create 
            try
            {
                myStream = new FileStream(path, FileMode.OpenOrCreate);
            }
            catch (NotSupportedException e)
            {}
            catch (DirectoryNotFoundException e)
            {}
            catch (Exception e)
            {
                if (!Directory.Exists("c:\\MonitoringSitesServiceTemp"))
                    Directory.CreateDirectory("C:\\MonitoringSitesServiceTemp");
                // for input file
                if (!File.Exists(path))
                    File.Create("c:\\MonitoringSitesServiceTemp\\NextQueriesTimetable.txt");
            }
            myStream.Close();
            //try to create a reader
            StreamReader strRead =
            new StreamReader(path,System.Text.Encoding.UTF8);
                
            //
            if (!String.IsNullOrEmpty(line = strRead.ReadLine()))
            {
                strRead.Close();
                string strAdv; string[] subs;
                DateTime neededTime, newNote;

                strAdv = line.Replace('.', ' ');
                line = strAdv.Replace(':', ' ');
                subs = line.Split(' ');

                // full the object DateTime
                neededTime = new DateTime(
                    Int32.Parse(subs[2]), // year
                    Int32.Parse(subs[1]), // month
                    Int32.Parse(subs[0]), // day
                    Int32.Parse(subs[3]), // hour
                    Int32.Parse(subs[4]), // minute
                    Int32.Parse(subs[5]));// second 

                // create a new time to query if it needs
                if (DateTime.Now > neededTime)
                {
                    // rewriting the time 
                    newNote = neededTime.AddDays(2);
                    neededTime = newNote;

                    // create writer
                    StreamWriter strWr =
                    new StreamWriter(path,false, System.Text.Encoding.UTF8);
                    strWr.WriteLine(newNote.ToString());
                    strWr.Close();
                }

                Result = neededTime;
            }
            else
            {
                strRead.Close();
                // create writer
                StreamWriter strWr =
                new StreamWriter(path, false, System.Text.Encoding.UTF8);
                DateTime t = new DateTime();
                DateTime toWr = new DateTime();
                t = DateTime.Now;
                int hour = t.Hour;
                int min = t.Minute;
                // case 1
                if (hour < 22)
                {
                    // год - месяц - день - час - минута - секунда
                    toWr =
                    new DateTime(t.Year, t.Month, t.Day, 22, 15, 00);
                    try
                    {
                        strWr.WriteLine(toWr.ToString());
                    }
                    catch (Exception e) { }
                }
                // case 2
                else if (hour == 22)
                {
                    if (min < 15)
                    {
                        toWr =
                        new DateTime(t.Year, t.Month, t.Day, 22, 15, 00);
                        strWr.WriteLine(toWr.ToString());
                    }
                    else if (min >= 15)
                    {
                        toWr =
                        new DateTime(t.Year, t.Month, t.Day + 1, 22, 15, 00);
                        strWr.WriteLine(toWr.ToString());
                    }
                }
                //case 3
                else
                {
                    toWr =
                    new DateTime(t.Year, t.Month, t.Day + 1, 22, 15, 00);
                    strWr.WriteLine(toWr.ToString());
                }
                strWr.Close();
                Result = toWr;
            }
            return Result;
        }
        public static bool SetTimes(DateTime diff,ref double Milisec)
        {
            TimeSpan diffTime = diff.Subtract(DateTime.Now);
            if (diffTime.TotalMinutes < 6)
            {
                Milisec = diffTime.TotalMilliseconds;
                return true;
            }
            else
                return false;
        }
        public void RunEventsManager()
        {
            // differ = GetTimeInfoFromFile();
            // try set watcher for query to Microsoft
            double milisec = 0.0;
            FirstTime = SetTimes(GetTimeInfoFromFile(), ref milisec);
            if (FirstTime)
            {
                FirstWatcher = new Timer();
                FirstWatcher.Interval = milisec;
                FirstWatcher.AutoReset = true;
                FirstWatcher.Elapsed += FirstWatcher_Elapsed;
                FirstWatcher.Start();
            }
            // set watcher for query to Google 
            SecondWatcher = new Timer();
            SecondWatcher.Interval = 4000.00;
            SecondWatcher.AutoReset = true;

            // set watcher for query to Apple
            ThirdWatcher = new Timer();
            ThirdWatcher.Interval = 7000.00;
            ThirdWatcher.AutoReset = true;
            //
            
            SecondWatcher.Elapsed += SecondWatcher_Elapsed;
            ThirdWatcher.Elapsed += ThirdWatcher_Elapsed;
            // start watchers
            
            SecondWatcher.Start();
            ThirdWatcher.Start();

        }
        public delegate void EventHandler();
        public event EventHandler RisingTimer;
        private void ThirdWatcher_Elapsed(object sender, ElapsedEventArgs e)
        {
            siteName = "http://Apple.com";
            RisingTimer();
            // try set watcher for query to Microsoft
            if (FirstTime == false)
            {
                double milisec = 0.0;
                bool NTime = SetTimes(GetTimeInfoFromFile(), ref milisec);
                if (NTime)
                {
                    FirstWatcher = new Timer();
                    FirstWatcher.Interval = milisec;
                    FirstWatcher.AutoReset = true;
                    FirstWatcher.Elapsed += FirstWatcher_Elapsed;
                    FirstWatcher.Start();
                }
            }
        }
        private void SecondWatcher_Elapsed(object sender, ElapsedEventArgs e)
        {
            siteName = "http://google.com";
            RisingTimer();
        }

        private void FirstWatcher_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (FirstTime == true) FirstTime = false;
            siteName = "http://Microsoft.com";
            RisingTimer();
        }

        public void StopEventsManager()
        {
            if(FirstWatcher != null) FirstWatcher.Stop();
            SecondWatcher.Stop();
            ThirdWatcher.Stop();
        }
        public EventsManager()
        {
            
        }

        public void SaveResponse(string statCode)
        {
        
            string path = @"C:\MonitoringSitesServiceTemp\HttpResponseTimetable.txt";
            // path to file output data
            Stream myStream = null;
            
            // open or create stream
            try
            {
                myStream = new FileStream(path, FileMode.OpenOrCreate);
            }
            catch (Exception e)
            {
                if (!Directory.Exists("C:\\MonitoringSitesServiceTemp"))
                    Directory.CreateDirectory("C:\\MonitoringSitesServiceTemp");
                
                if (!File.Exists(path))
                    File.Create("C:\\MonitoringSitesServiceTemp\\HttpResponseTimetable.txt");
        
            }
            myStream.Close();
            DateTime tNow = DateTime.Now;
            try
            {
                StreamWriter strWr =
                new StreamWriter(path, true, System.Text.Encoding.UTF8);
                strWr.WriteLine(tNow.ToString("F") + " HttpStatusCode: " + statCode);
                strWr.Close();
            }
            catch (Exception e) { }
            
        }
        public string getSiteName() { return siteName; }
    }
    
}
