using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace PerformanceMonitorC
{
    class Program
    {
        static void Main(string[] args)
        {
            #region speech
            //This is the region to greet the user
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Rate = 2;
            #endregion
            #region Performance Counters
            //This is the region to gather data from PerfMon.exe
            PerformanceCounter perfCpuCount = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            PerformanceCounter perfMemCount = new PerformanceCounter("Memory", "Available MBytes");
            PerformanceCounter perfUpTCount = new PerformanceCounter("System", "System Up Time");
            #endregion


            while (true)
            {
                #region Static Performance Counters
                //This takes dynamic data from PerfMon.exe and makes it static to provide consistancy for the total 5 seconds of loop
                float currentCpuCount = perfCpuCount.NextValue();
                float currentMemCount = perfMemCount.NextValue();
                float currentSysCount = perfUpTCount.NextValue();
                //Wanted to convert precent into whole number to minimize voice synth later
                int currentCpuCountInt = (int)currentCpuCount;
                //Wanted to convert MB into GB
                float currentMemCountGB = currentMemCount / 1024;
                //Wanted to convert seconds into hours to make easier for end user
                float currentSysCountH = currentSysCount / 3600;
                int currentSysCountHours = (int)currentSysCountH;
                #endregion

                //This loop is to show and loop (every 5 sec) for user to see
                Console.WriteLine("CUP Load: {0}%", currentCpuCount);
                Console.WriteLine("Avai Mem: {0}GB", currentMemCountGB);
                Console.WriteLine("Sys Time: {0}Hours", currentSysCountHours);

                if (currentCpuCountInt > 90)
                {
                    string cpuLoadVocalMessage = string.Format("The CPU load is very high!");
                    synth.Speak(cpuLoadVocalMessage);
                }
                if (currentMemCount < 1024)
                {
                    string memLoadVocalMessage = string.Format("The system is running low on memory!");
                    synth.Speak(memLoadVocalMessage);
                }
                Thread.Sleep(5000);
            }
        }
    }
}
