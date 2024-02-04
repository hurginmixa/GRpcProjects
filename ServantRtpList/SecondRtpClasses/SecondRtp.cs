using System;
using System.Windows.Forms;
using GRpc.API;

namespace ServantRtpList.SecondRtpClasses
{
    [Serializable]
    public class SecondRtp : MarshalByRefObject, IRtpWindowOpener
    {
        public bool Open(IntPtr ownerHwnd)
        {
            SecondRtpMainForm form = new SecondRtpMainForm();
            IWin32Window owner = NativeWindow.FromHandle(ownerHwnd);

            DialogResult dialogResult = form.ShowDialog(owner);

            return dialogResult == DialogResult.OK;
        }
    }
}