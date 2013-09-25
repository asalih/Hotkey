using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace HotKey
{
    public partial class Form1 : Form
    {

        public delegate void Killer(ref Thread t);
        int found = 0;
        int pro = 0;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myLib.myHotKey hotkey = new myLib.myHotKey();
            //Alt+Down Arrow
            //kills webdev process
            hotkey.Shortcut = Shortcut.AltDownArrow;
            hotkey.Pressed += new EventHandler(hotkey_Pressed);
        }

        void hotkey_Pressed(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            if (processes != null && processes.Count() > 0)
            {
                foreach (Process item in processes)
                {
                    if (item.ProcessName.ToLower().Contains("webdev.webserver"))
                    {
                        ThreadStart ts = new ThreadStart(item.Kill);
                        Thread t = new Thread(ts);
                        Killer k = new Killer(CheckState);
                        k.BeginInvoke(ref t, CallBack, null);
                    }
                }

                if (found < 1)
                {
                    label1.Text = "Couldnt find process!";
                }
            }
        }

        public void CheckState(ref Thread th)
        {
            pro++;
            th.Start();
            label1.Text = "Working...";

        }

        public void CallBack(IAsyncResult ar)
        {
            if (ar.IsCompleted == true)
            {
                pro--;
                if (pro == 0)
                {
                    label1.Text = "All jobs are done!";
                }
                else
                {
                    label1.Text = "...";
                }
            }
        }



    }
}
