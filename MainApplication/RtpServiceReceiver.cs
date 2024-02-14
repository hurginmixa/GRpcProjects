using System;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Grpc.Core;
using GRpc.API;
using RtpServiceClasses;

namespace MainApplication
{
    public class RtpServiceReceiver : RtpServiceClasses.RtpServiceReceiver.RtpServiceReceiverBase
    {
        public Form1 MainForm;

        public RtpServiceReceiver()
        {
            MainForm = null;
        }

        public override Task<GrpcShowRtpReceiverReplay> ShowRtp(GrpcShowRtpReceiverRequest request, ServerCallContext context)
        {
            IRtpOption rtpOption = RtpOption.Make(request.RptParams);

            ShowRtpInOtherDomain(request.DllPath, request.ClassName, rtpOption, MainForm);
            
            GrpcShowRtpReceiverReplay replay = new GrpcShowRtpReceiverReplay();
            replay.Status = GrpcOperationStatus.Ok;
            replay.RptParams = RtpOptionMapper.ToGrpcMapping(rtpOption);

            return Task.FromResult(replay);
        }

        public static void ShowRtpInOtherDomain(string dllPath, string className, IRtpOption rtpOption, Control mainForm)
        {
            var id = Guid.NewGuid();
            var appDomain = AppDomain.CreateDomain(id.ToString(), (Evidence) null, Path.GetDirectoryName(dllPath), Path.GetDirectoryName(dllPath), false);

            try
            {
                void MM()
                {
                    IRtpWindowOpener inst = (IRtpWindowOpener)appDomain.CreateInstanceFromAndUnwrap(dllPath, className);

                    inst.Open(mainForm.Handle, rtpOption);
                }

                mainForm.Invoke((Action) MM);
            }
            finally
            {
                AppDomain.Unload(appDomain);
            }
        }
    }
}