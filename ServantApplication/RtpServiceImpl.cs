using System;
using System.Threading.Tasks;
using GRpc.API;
using Grpc.Core;
using RtpServiceClasses;
using ServantRtpList;
using ServantRtpList.FirstRtpClasses;
using ServantRtpList.SecondRtpClasses;

namespace ServantApplication
{
    internal class RtpServiceImpl : RtpService.RtpServiceBase
    {
        private RtpServiceReceiver.RtpServiceReceiverClient _receiverClient;

        public override Task<GrpcHandChackingReplay> HandChacking(GrpcHandChackingRequest request, ServerCallContext context)
        {
            _receiverClient = Helpers.MakeRtpServiceReceiverClient(request.HostName, request.PortNumber);

            GrpcHandChackingReplay replay = new GrpcHandChackingReplay();
            replay.Status = GrpcOperationStatus.Ok;

            return Task.FromResult(replay);
        }

        public override Task<GrpcShowRtpReply> ShowRtp(GrpcShowRtpRequest request, ServerCallContext context)
        {
            IRtpOption rtpOption = RtpOption.Make(request.RptParams);

            string rtpName = rtpOption.Item(key: "RtpName");

            switch (rtpName)
            {
                case "First Rtp":
                {
                    return ShowRtp(request: request, type: typeof(FirstRtp));
                }

                case "Second Rtp":
                {
                    return ShowRtp(request: request, type: typeof(SecondRtp));
                }
            }

            return Task.FromResult(result: new GrpcShowRtpReply());
        }

        public override Task<GrpcRtpInfoReplay> GetRtpInfo(GrpcRtpInfoRequest request, ServerCallContext context)
        {
            switch (request.RtpInfoRequestParameter)
            {
                case "First Rtp":
                {
                    return MakeRtpInfoReplay(request: request, type: typeof(FirstRtp));
                }

                case "Second Rtp":
                {
                    return MakeRtpInfoReplay(request: request, type: typeof(SecondRtp));
                }
            }

            return base.GetRtpInfo(request, context);
        }

        private Task<GrpcRtpInfoReplay> MakeRtpInfoReplay(GrpcRtpInfoRequest request, Type type)
        {
            GrpcRtpInfoReplay replay = new GrpcRtpInfoReplay();
            replay.DllPath = type.Assembly.Location;
            replay.ClassName = type.FullName;

            return Task.FromResult(replay);
        }

        private Task<GrpcShowRtpReply> ShowRtp(GrpcShowRtpRequest request, Type type)
        {
            GrpcShowRtpReceiverRequest reciverRequest = new GrpcShowRtpReceiverRequest();
            reciverRequest.DllPath = type.Assembly.Location;
            reciverRequest.ClassName = type.FullName;
            reciverRequest.RptParams = request.RptParams;

            GrpcShowRtpReceiverReplay receiverReplay = _receiverClient.ShowRtp(reciverRequest);

            GrpcShowRtpReply rtpReply = new GrpcShowRtpReply();
            rtpReply.Status = receiverReplay.Status;
            rtpReply.RptParams = receiverReplay.RptParams;

            return Task.FromResult(rtpReply);
        }
    }
}