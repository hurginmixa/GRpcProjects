using RtpServiceClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainApplication
{
    public partial class Form1 : Form
    {
        private readonly RtpService.RtpServiceClient _client;

        public Form1(RtpService.RtpServiceClient client)
        {
            _client = client;

            InitializeComponent();
        }

        private async void btShowRtp_Click(object sender, EventArgs e)
        {
            ShowRtpRequest request = new ShowRtpRequest();
            request.RptParams = (string) lbRtpList.SelectedItem;
            
            ShowRtpReply showRtpReply = await _client.ShowRtpAsync(request);
        }

        private void OnLoad(object sender, EventArgs e)
        {
            lbRtpList.Items.Add("First Rtp");
            lbRtpList.Items.Add("Second Rtp");

            lbRtpList.SelectedIndex = 0;
        }
    }
}
