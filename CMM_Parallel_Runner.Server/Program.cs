using CMM_Parallel_Runner.API;
using CMM_Parallel_Runner.GRpc.Classes;
using System;
using System.Threading;
using System.Windows.Forms;

namespace CMM_Parallel_Runner.Server
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CmmProcessor cmmProcessor = new CmmProcessor();

            GrpcService grpcService = new GrpcService(cmmProcessor);

            GrpcServer server = new GrpcServer(CMM_Parallel_Runner_Grpc_Service.BindService(grpcService), Helpers.DefaultHost, Helpers.DefaultPort);
            server.Start();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            finally
            {
                server.Stop();
            }
        }
    }

    internal static class Helpers
    {
        public const string DefaultHost = "localhost";

        public const int DefaultPort = 50051;
    }
}
