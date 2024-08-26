using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CMM_Parallel_Runner.API;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;
using GrpcClient = CMM_Parallel_Runner.GRpc.Classes.CMM_Parallel_Runner_Grpc_Service.CMM_Parallel_Runner_Grpc_ServiceClient;

namespace CMM_Parallel_Runner.Client
{
    public partial class Form1 : Form
    {
        private Task _readStreamTask;
        private CancellationTokenSource _cancellationTokenSource;
        private GrpcClient _client;
        private int _num;
        private CancellationToken _token;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Channel channel =  Helpers.MakeChannel(Helpers.DefaultHost, Helpers.DefaultPort);
            _client = new GrpcClient(channel);

            _num = 0;

            _cancellationTokenSource = new CancellationTokenSource();
            _token = _cancellationTokenSource.Token;

            _readStreamTask = ReadingStreamTask();
        }

        private async Task ReadingStreamTask()
        {
            AsyncServerStreamingCall<GrpcExportResult> handShaking = _client.GrpcHandShaking(new GrpcHandShakingRequest() {RequestorName = "Mixa"});

            IAsyncStreamReader<GrpcExportResult> stream = handShaking.ResponseStream;

            while (await stream.MoveNext(_token))
            {
                GrpcExportResult current = stream.Current;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            _readStreamTask.Wait();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var r1 = _client.GrpcDoCmmExport(new GrpcCmmExportRequest() {Index = _num++});
        }
    }
}
