using System;
using System.Windows.Forms;
using GRpc.API;

namespace GRpc.Server
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GreetServer server = new GreetServer();
            server.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            server.Stop();
        }
    }
}
