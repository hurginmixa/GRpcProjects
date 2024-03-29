﻿using System;
using System.Windows.Forms;
using GRpc.API;
using GrpcGreeter;

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
            GrpcServer server = new GrpcServer(Greeter.BindService(new GreetServiceImpl()), Helpers.DefaultHost, Helpers.DefaultPort);
            server.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            server.Stop();
        }
    }
}
