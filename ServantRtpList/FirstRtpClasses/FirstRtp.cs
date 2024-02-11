using System;
using System.Windows.Forms;
using GRpc.API;

namespace ServantRtpList.FirstRtpClasses
{
    [Serializable]
    public class FirstRtp : MarshalByRefObject, IRtpWindowOpener
    {
        public bool Open(IntPtr ownerHwnd, IRtpOption inOption)
        {
            FirstRtpMainForm form = new FirstRtpMainForm(inOption.Item("Data"));
            IWin32Window owner = NativeWindow.FromHandle(ownerHwnd);

            DialogResult dialogResult = form.ShowDialog(owner);

            bool isOk = dialogResult == DialogResult.OK;

            if (isOk)
            {
                inOption.SetItem("Data", form.EnteredText);
            }

            return isOk;
        }
    }
}
