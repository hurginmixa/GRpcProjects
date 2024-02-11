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
        private RtpServiceReceiver.RtpServiceReceiverClient _client;

        public override Task<GrpcHandChackingReplay> HandChacking(GrpcHandChackingRequest request, ServerCallContext context)
        {
            _client = Helpers.MakeRtpServiceReceiverClient(request.HostName, request.PortNumber);

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

        private Task<GrpcShowRtpReply> ShowRtp(GrpcShowRtpRequest request, Type type)
        {
            GrpcShowRtpReceiverRequest reciverRequest = new GrpcShowRtpReceiverRequest();
            reciverRequest.DllPath = type.Assembly.Location;
            reciverRequest.ClassName = type.FullName;
            reciverRequest.RptParams = request.RptParams;

            GrpcShowRtpReceiverReplay receiverReplay = _client.ShowRtp(reciverRequest);

            GrpcShowRtpReply rtpReply = new GrpcShowRtpReply();
            rtpReply.Status = receiverReplay.Status;
            rtpReply.RptParams = receiverReplay.RptParams;

            return Task.FromResult(rtpReply);
        }
    }
}