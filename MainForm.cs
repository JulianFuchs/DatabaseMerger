using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DatabaseMerger
{
    public partial class MainForm : Form
    {
        DebugForm debugForm;
        private bool anyButtonClicked;

        public MainForm()
        {
            InitializeComponent();
            anyButtonClicked = false;
            debugForm = new DebugForm();
            Thread newThread = new Thread(() => Application.Run(debugForm));
            newThread.Start();

            Program.mainForm = this;
        }

        private void StartMergingButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (!anyButtonClicked)
            {
                anyButtonClicked = true;

                Thread newThread = new Thread(new ThreadStart(startMerging));
                newThread.Start();
            }
        }

        private void startMerging()
        {
            Program.debug("Starting to merge databases:");

            Merger merger = new Merger();

            Thread newThread = new Thread(new ThreadStart(merger.start));
            newThread.Start();

            newThread.Join();
            anyButtonClicked = false;
            

        }

        public void setTimeRunning (TimeSpan time)
        {
            int seconds = time.Seconds;
            int minutes = time.Minutes;
            int hours = time.Hours;
            int days = time.Days;
            TimeRunningLabel.Text = "Time Running: " + days + " days; " + hours + " hours; " + minutes + " minutes; and " + seconds + " seconds";
        }

        public void setCurrentId(int id)
        {
            CurrentIdLabel.Text = "Currently looking at id: " + id;
        }

        public void setProgressLabel(int current)
        {
            ProgressLabel.Text = "Progress: " + current + "/" + Program.foundTorrents;
            setProgressBar(current);
        }

        public void setProgressBar(int current)
        {
            MergerProgressBar.Maximum = Program.foundTorrents;
            MergerProgressBar.Value = current; //TODO: crash: Argument out of range exception
        }
    }
}
