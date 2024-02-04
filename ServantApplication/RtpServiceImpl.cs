using System;
using System.Threading.Tasks;
using GRpc.API;
using Grpc.Core;
using RtpServiceClasses;
using ServantRtpList.FirstRtpClasses;
using ServantRtpList.SecondRtpClasses;

namespace ServantApplication
{
    internal class RtpServiceImpl : RtpService.RtpServiceBase
    {
        private RtpServiceReceiver.RtpServiceReceiverClient _client;

        public override Task<HandChackingReplay> HandChacking(HandChackingRequest request, ServerCallContext context)
        {
            _client = Helpers.MakeRtpServiceReceiverClient(request.HostName, request.PortNumber);

            HandChackingReplay replay = new HandChackingReplay();
            replay.Status = OperationStatus.Ok;

            return Task.FromResult(replay);
        }

        public override Task<ShowRtpReply> ShowRtp(ShowRtpRequest request, ServerCallContext context)
        {
            switch (request.RptParams)
            {
                case "First Rtp":
                {
                    return ShowRtp(request, typeof(FirstRtp));
                }

                case "Second Rtp":
                {
                    return ShowRtp(request, typeof(SecondRtp));
                }
            }

            return Task.FromResult(new ShowRtpReply());
        }

        private Task<ShowRtpReply> ShowRtp(ShowRtpRequest request, Type type)
        {
            ShowRtpReceiverRequest reciverRequest = new ShowRtpReceiverRequest();
            reciverRequest.DllPath = type.Assembly.Location;
            reciverRequest.ClassName = type.FullName;
            reciverRequest.RptParams = request.RptParams;

            ShowRtpReceiverReplay receiverReplay = _client.ShowRtp(reciverRequest);

            ShowRtpReply rtpReply = new ShowRtpReply();
            rtpReply.Status = receiverReplay.Status;
            rtpReply.RptParams = receiverReplay.RptParams;

            return Task.FromResult(rtpReply);
        }
    }
}