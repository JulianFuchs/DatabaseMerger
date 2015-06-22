using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace DatabaseMerger
{
    static class Program
    {
        static public DebugForm debugForm { set; get; }
        static public MainForm mainForm;
        static public Thread threadsManager; // both not needed?
        static private Thread formsManager;

        static public int foundTorrents;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            threadsManager = Thread.CurrentThread;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // create Thread to  run MainForm
            Thread newThread = new Thread(() => Application.Run(new MainForm()));
            newThread.Start();

            formsManager = newThread;

            Thread.Sleep(1000); //fishy
            debug("This is the main thread with id: " + Thread.CurrentThread.ManagedThreadId + ". He spwaned the Thread responsible for the formWindows with id: "
                + formsManager.ManagedThreadId);
        }

        private delegate void logPrint(string str);
        static public void debug(String s)
        {
            logPrint lP = new logPrint(debugForm.addDebugLog);
            debugForm.Invoke(lP, s);

            //debugForm.addDebugLog(s);
        }

        private delegate void mainTimeUpdate(TimeSpan i);
        static public void updateTimeRunning(TimeSpan time)
        {
            mainTimeUpdate timeUpdate = new mainTimeUpdate(mainForm.setTimeRunning);
            mainForm.Invoke(timeUpdate, time);
        }

        private delegate void mainCurrentId(int id);
        static public void setCurrentIdLabel(int id)
        {
            mainCurrentId idUpdate = new mainCurrentId(mainForm.setCurrentId);
            mainForm.Invoke(idUpdate, id);
        }

        private delegate void mainProgressLabel(int current);
        static public void setProgressLabel(int current)
        {
            mainProgressLabel pu = new mainProgressLabel(mainForm.setProgressLabel);
            mainForm.Invoke(pu,current);
        }
    }
}
