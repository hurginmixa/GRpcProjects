using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMM_Parallel_Runner.Client
{
    public partial class Form1 : Form
    {
        private int _num;
        
        private Client _client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _num = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopClient();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_client == null)
            {
                _client = new Client();
            }

            _client.SendRequest(++_num);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopClient();
        }

        private void StopClient()
        {
            if (_client == null)
            {
                return;
            }

            _client.Stop();
            _client = null;
        }
    }
}
