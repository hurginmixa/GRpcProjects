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
            var id = Guid.NewGuid();
            var appDomain = AppDomain.CreateDomain(id.ToString(), (Evidence) null, Path.GetDirectoryName(request.DllPath), Path.GetDirectoryName(request.DllPath), false);

            try
            {
                var rtpOption = RtpOption.Make(request.RptParams);

                void MM()
                {
                    IRtpWindowOpener inst = (IRtpWindowOpener)appDomain.CreateInstanceFromAndUnwrap(request.DllPath, request.ClassName);

                    inst.Open(MainForm.Handle, rtpOption);
                }

                MainForm.Invoke((Action) MM);

                GrpcShowRtpReceiverReplay replay = new GrpcShowRtpReceiverReplay();
                replay.Status = GrpcOperationStatus.Ok;
                replay.RptParams = RtpOptionMapper.ToGrpcMapping(rtpOption);

                return Task.FromResult(replay);
            }
            finally
            {
                AppDomain.Unload(appDomain);
            }
        }
    }
}