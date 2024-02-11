using System.Windows.Forms;

namespace ServantRtpList.FirstRtpClasses
{
    public partial class FirstRtpMainForm : Form
    {
        private readonly string _initText;

        public FirstRtpMainForm(string initText)
        {
            _initText = initText;
            InitializeComponent();
        }

        private void btOk_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FirstRtpMainForm_Load(object sender, System.EventArgs e)
        {
            tbText.Text = _initText;
        }

        public string EnteredText => tbText.Text;
    }
}
