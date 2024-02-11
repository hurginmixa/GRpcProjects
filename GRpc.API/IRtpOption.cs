using System.Collections.Generic;
using System.Linq;
using RtpServiceClasses;

namespace GRpc.API
{
    public interface IRtpOption
    {
        string[] Keys { get; }

        string Item(string key);

        void SetItem(string key, string value);
    }

    public static class RtpOptionMapper
    {
        public static GrpcRtp ToGrpcMapping(IRtpOption src)
        {
            GrpcRtp res = new GrpcRtp();

            res.Parameters.Add(src.Keys.Select(key => new GrpcRtpParamerter {Name = key, Value = src.Item(key)}));

            return res;
        }

        public static void FromGrpcMapping(GrpcRtp src, IRtpOption dest)
        {
            foreach (GrpcRtpParamerter grpcRtpParameter in src.Parameters)
            {
                dest.SetItem(key: grpcRtpParameter.Name, value: grpcRtpParameter.Value);
            }
        }
    }
}