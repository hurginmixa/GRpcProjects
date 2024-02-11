using System;

namespace GRpc.API
{
    public interface IRtpWindowOpener
    {
        bool Open(IntPtr ownerHwnd, IRtpOption inOption);
    }
}
