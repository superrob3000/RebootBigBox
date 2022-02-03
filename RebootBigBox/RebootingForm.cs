using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RebootBigBox
{
    public partial class RebootingForm : Form
    {
        public RebootingForm()
        {
            InitializeComponent();

            Task.Factory.StartNew(() => { this.Reboot(); });
        }

        private void Reboot()
        {
            //Kill BigBox process
            {
                Process[] Processes = System.Diagnostics.Process.GetProcesses();
                for (int i = 0; i < Processes.Length; i++)
                {
                    if (Processes[i].ProcessName.StartsWith("LaunchBox") ||
                        Processes[i].ProcessName.StartsWith("BigBox"))
                    {
                        Processes[i].Kill();
                    }
                }
            }

            //Check if BigBox is still running
            bool BigBoxRunning;
            do
            {
                Thread.Sleep(1000);

                BigBoxRunning = false;
                Process[] Processes = System.Diagnostics.Process.GetProcesses();

                for (int i = 0; i < Processes.Length; i++)
                {
                    if (Processes[i].ProcessName.StartsWith("LaunchBox") ||
                        Processes[i].ProcessName.StartsWith("BigBox"))
                    {
                        BigBoxRunning = true;
                    }
                }

            } while (BigBoxRunning);

            //Restart BigBox
            Process ps_bigbox = null;
            ps_bigbox = new Process();
            ps_bigbox.StartInfo.UseShellExecute = false;
            ps_bigbox.StartInfo.RedirectStandardInput = false;
            ps_bigbox.StartInfo.RedirectStandardOutput = false;
            ps_bigbox.StartInfo.CreateNoWindow = true;
            ps_bigbox.StartInfo.UserName = null;
            ps_bigbox.StartInfo.Password = null;
            ps_bigbox.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            ps_bigbox.StartInfo.FileName = Path.GetDirectoryName(Application.ExecutablePath).ToString() + "/BigBoxWithStartupMarquee.exe";

            if(!File.Exists(ps_bigbox.StartInfo.FileName))
                ps_bigbox.StartInfo.FileName = Path.GetDirectoryName(Application.ExecutablePath).ToString() + "/BigBox.exe";

            ps_bigbox.Start();


            //Exit
            Thread.Sleep(4000);
            this.Close();
        }
    }
}
