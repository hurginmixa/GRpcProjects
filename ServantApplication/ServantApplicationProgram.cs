using GRpc.API;
using RtpServiceClasses;
using System;
using System.Windows.Forms;

namespace ServantApplication
{
    internal static class ServantApplicationProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GrpcServer server = new GrpcServer(RtpService.BindService(new RtpServiceImpl()), Helpers.DefaultHost, Helpers.DefaultPort);
            server.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
