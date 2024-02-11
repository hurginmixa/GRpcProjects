using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using GRpc.API;
using RtpServiceClasses;

namespace MainApplication
{
    internal static class MainApplicationProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            const string servantFileName = "ServantApplication";
            const string servantFilePath = @"C:\Mixa\GRpcProjects\ServantApplication\bin\Debug";

            RtpServiceReceiver serviceReceiver = new RtpServiceReceiver();

            GrpcServer server = new GrpcServer(RtpServiceClasses.RtpServiceReceiver.BindService(serviceReceiver), Helpers.DefaultHost, Helpers.ReceiverPort);
            server.Start();

            Process[] processes = Process.GetProcessesByName(servantFileName);

            Process process = null;

            if (processes.Length == 0)
            {
                process = Process.Start(Path.Combine(servantFilePath, servantFileName) + ".exe");
            }

            try
            {
                Thread.Sleep(100);

                RtpService.RtpServiceClient client = Helpers.MakeRtpServiceClient(Helpers.DefaultHost, Helpers.DefaultPort);


                GrpcHandChackingRequest handCheckingRequest = new GrpcHandChackingRequest();
                handCheckingRequest.HostName = Helpers.DefaultHost;
                handCheckingRequest.PortNumber = Helpers.ReceiverPort;

                GrpcHandChackingReplay handChackingReplay = client.HandChacking(handCheckingRequest);


                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
            
                Form1 mainForm = new Form1(client);
                serviceReceiver.MainForm = mainForm;

                Application.Run(mainForm);
            }
            finally
            {
                process?.Kill();
            }
        }
    }
}
