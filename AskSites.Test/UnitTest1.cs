using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AskSites;

namespace AskSites.Test
{
    [TestClass]
    public class UnitTest1
    {
        private AskSites.EventsManager getTestObject()
        {
            return new EventsManager();
        }
        [TestMethod]
        public void SetTimes_StartWatcher()
        {
            // arrange
            var myTarget = getTestObject();
       
            DateTime tPrep = DateTime.Now;
            DateTime tPrepare =  tPrep.AddMinutes(1); 
            // act
            double milisec=0.0;
            bool myResult = EventsManager.SetTimes(tPrepare ,ref milisec);
            // assert
            Assert.AreEqual(true, myResult);
        }
        [TestMethod]
        public void SetTimes_DoNotStartWatcher()
        {
            DateTime tPrep = DateTime.Now;
            DateTime tPrepare = tPrep.AddMinutes(7);
            // act
            double milisec = 0.0;
            bool myResult = EventsManager.SetTimes(tPrepare, ref milisec);
            // assert
            Assert.AreEqual(false, myResult);
        }
    }
}
