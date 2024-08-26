using System;
using System.Collections.Generic;
using CMM_Parallel_Runner.API.Contracts;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;

namespace CMM_Parallel_Runner.API
{
    public class SessionDictionary
    {
        private readonly Dictionary<string, Session> _sessions = new Dictionary<string, Session>();

        public event Session.SessionDelegate OnAddingNewSession;
        public event Session.SessionDelegate OnRemovingSession;
        public event Session.SessionAddDelegate OnChangingSession;

        public Session AddNewSession(string sessionId, ICmmProcessor cmmProcessor, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
            lock (_sessions)
            {
                if (_sessions.TryGetValue(sessionId, out var session))
                {
                    throw new Exception($"The session Id {sessionId} already exists");
                }

                session = new Session(sessionId, cmmProcessor, responseStream, context);
                session.OnSessionChanged += OnSessionOnOnSessionChanged;

                _sessions.Add(sessionId, session);

                OnAddingNewSession?.Invoke(sessionId, session);

                return session;
            }
        }

        private void OnSessionOnOnSessionChanged(string id, Session session1, bool add)
        {
            OnChangingSession?.Invoke(id, session1, add);
        }

        public Session GetSession(string sessionId)
        {
            lock (_sessions)
            {
                if (!_sessions.TryGetValue(sessionId, out var session))
                {
                    throw new Exception($"The session Id {sessionId} was not found");
                }

                return session;
            }
        }

        public void Remove(string sessionId)
        {
            lock (_sessions)
            {
                Session session = GetSession(sessionId);
                session.OnSessionChanged -= OnSessionOnOnSessionChanged;

                OnRemovingSession?.Invoke(sessionId, session);

                session.Dispose();

                _sessions.Remove(sessionId);
            }
        }
    }
}