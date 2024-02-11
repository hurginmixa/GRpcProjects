using RtpServiceClasses;
using System;
using System.Windows.Forms;
using GRpc.API;

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
            IRtpOption rtpOption = new RtpOption();
            rtpOption.SetItem("RtpName", (string) lbRtpList.SelectedItem);
            rtpOption.SetItem("Data", tbText.Text);

            GrpcShowRtpRequest request = new GrpcShowRtpRequest();
            request.RptParams = RtpOptionMapper.ToGrpcMapping(rtpOption);
            
            GrpcShowRtpReply showRtpReply = await _client.ShowRtpAsync(request);

            IRtpOption option = RtpOption.Make(showRtpReply.RptParams);
            tbText.Text = option.Item("Data");
        }

        private void OnLoad(object sender, EventArgs e)
        {
            lbRtpList.Items.Add("First Rtp");
            lbRtpList.Items.Add("Second Rtp");

            lbRtpList.SelectedIndex = 0;

            tbText.Text = string.Empty;
        }
    }
}
