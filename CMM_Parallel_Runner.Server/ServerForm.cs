using CMM_Parallel_Runner.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMM_Parallel_Runner.Server
{
    public partial class ServerForm : Form
    {
        private readonly GrpcService _grpcService;
        private int _count;

        public ServerForm(GrpcService grpcService)
        {
            _grpcService = grpcService;
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _grpcService.Sessions.OnAddingNewSession += OnAddingNewSession;
            _grpcService.Sessions.OnRemovingSession += OnRemovingSession;
            _count = 0;
        }

        private void OnRemovingSession(string id, Session session)
        {
            _count--;

            Invoke((Action) (() => { lbCount.Text = _count.ToString(); }));
        }

        private void OnAddingNewSession(string id, Session session)
        {
            _count++;

            Invoke((Action) (() => { lbCount.Text = _count.ToString(); }));
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _grpcService.Sessions.OnAddingNewSession += OnAddingNewSession;
            _grpcService.Sessions.OnRemovingSession += OnRemovingSession;
        }
    }
}
