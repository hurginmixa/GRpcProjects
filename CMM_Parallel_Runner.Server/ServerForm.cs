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
            _count = 0;
            
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _grpcService.Sessions.OnAddingNewSession += OnAddingNewSession;
            _grpcService.Sessions.OnRemovingSession += OnRemovingSession;
            _grpcService.Sessions.OnChangingSession += OnChangingSession;
            lbCount.Text = _count.ToString();
        }

        private void OnChangingSession(string id, Session session, bool add)
        {
            Invoke((Action) (() =>
            {
                lboxSessionList.BeginUpdate();

                SessionItem[] items = lboxSessionList.Items.Cast<SessionItem>().ToArray();

                lboxSessionList.Items.Clear();

                foreach (SessionItem item in items)
                {
                    lboxSessionList.Items.Add(item);
                }

                lboxSessionList.EndUpdate();
            }));
        }

        private void OnRemovingSession(string id, Session session)
        {
            _count--;

            Invoke((Action) (() =>
            {
                lbCount.Text = _count.ToString();

                SessionItem item = lboxSessionList.Items.Cast<SessionItem>().First(i => i.Session == session);
                lboxSessionList.Items.Remove(item);
            }));
        }

        private void OnAddingNewSession(string id, Session session)
        {
            _count++;

            Invoke((Action) (() =>
            {
                lbCount.Text = _count.ToString();

                lboxSessionList.Items.Add(new SessionItem(session));
            }));
        }

        class SessionItem
        {
            public SessionItem(Session session)
            {
                Session = session;
            }

            public readonly Session Session;

            public override string ToString() => $"{Session.SessionId} : {Session.RequestCount}";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _grpcService.Sessions.OnAddingNewSession -= OnAddingNewSession;
            _grpcService.Sessions.OnRemovingSession -= OnRemovingSession;
            _grpcService.Sessions.OnChangingSession -= OnChangingSession;
        }
    }
}
