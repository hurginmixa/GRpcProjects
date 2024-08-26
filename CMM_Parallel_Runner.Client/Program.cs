using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMM_Parallel_Runner.Client
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Process[] processesByName = Process.GetProcessesByName("CMM_Parallel_Runner.Server");
            if (processesByName.Length == 0)
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo(@"..\..\..\CMM_Parallel_Runner.Server\bin\Debug\CMM_Parallel_Runner.Server.exe");
                process.Start();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
