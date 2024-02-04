using System;
using System.Windows.Forms;
using GRpc.API;

namespace ServantRtpList.FirstRtpClasses
{
    [Serializable]
    public class FirstRtp : MarshalByRefObject, IRtpWindowOpener
    {
        public bool Open(IntPtr ownerHwnd)
        {
            FristRtpMainForm form = new FristRtpMainForm();
            IWin32Window owner = NativeWindow.FromHandle(ownerHwnd);

            DialogResult dialogResult = form.ShowDialog(owner);

            return dialogResult == DialogResult.OK;
        }
    }
}
