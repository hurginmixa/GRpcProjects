using Grpc.Core;
using System.Collections.Generic;
using RtpServiceClasses;

namespace GRpc.API
{
    public static class Helpers
    {
        private const int GRPC_MAX_RECEVIE_MESSAGE_LEHGTH = (4 * 1024 * 1024) * 3;

        public const string DefaultHost = "localhost";
        public const int DefaultPort = 50051;
        public const int ReceiverPort = 50052;

        public static Channel MakeChannel(string host, int port)
        {
            var channelOptions = new List<ChannelOption>
            {
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, GRPC_MAX_RECEVIE_MESSAGE_LEHGTH)
            };
            
            var channel = new Channel($"{host}:{port}", ChannelCredentials.Insecure, channelOptions);
            return channel;
        }

        public static RtpServiceReceiver.RtpServiceReceiverClient MakeRtpServiceReceiverClient(string host, int port)
        {
            Channel channel = MakeChannel(host, port);
            return new RtpServiceReceiver.RtpServiceReceiverClient(channel);
        }

        public static RtpService.RtpServiceClient MakeRtpServiceClient(string host, int port)
        {
            Channel channel = MakeChannel(host, port);
            return new RtpService.RtpServiceClient(channel);
        }
    }
}
