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

        public delegate void SessionDelegate(string sessionId, Session session);
        public event SessionDelegate OnAddingNewSession;
        public event SessionDelegate OnRemovingSession;

        public Session AddNewSession(string sessionId, ICmmProcessor cmmProcessor, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
            lock (_sessions)
            {
                if (_sessions.TryGetValue(sessionId, out var session))
                {
                    throw new Exception($"The session Id {sessionId} already exists");
                }

                session = new Session(cmmProcessor, responseStream, context);
                _sessions.Add(sessionId, session);

                OnAddingNewSession?.Invoke(sessionId, session);

                return session;
            }
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
                var session = GetSession(sessionId);

                OnRemovingSession?.Invoke(sessionId, session);

                session.Dispose();

                _sessions.Remove(sessionId);
            }
        }
    }
}