using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatabaseMerger
{
    public partial class DebugForm : Form
    {
        public delegate void DebugDelegate(string s);

        public DebugForm()
        {
            InitializeComponent();
            Program.debugForm = this;
        }

        public void addDebugLog(String s)
        {
            debugLog.Text += s + "\n";
            //debugLog.Text += "Thread with id: " + Thread.CurrentThread.ManagedThreadId + " writes to the text box" + "\n\n";
        }
    }
}
