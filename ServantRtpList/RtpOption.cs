using System;
using GRpc.API;
using RtpServiceClasses;
using System.Collections.Generic;
using System.Linq;

namespace ServantRtpList
{
    [Serializable]
    public class RtpOption : MarshalByRefObject,  IRtpOption
    {
        private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

        public string[] Keys => _values.Keys.ToArray();

        public string Item(string key) => _values.TryGetValue(key, out var value) ? value : string.Empty;

        public void SetItem(string key, string value) => _values[key] = value;

        public static IRtpOption Make(GrpcRtp rptParams)
        {
            IRtpOption rtpOption = new RtpOption();
            RtpOptionMapper.FromGrpcMapping(rptParams, rtpOption);
            return rtpOption;
        }
    }
}
