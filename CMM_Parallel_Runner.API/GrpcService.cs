using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CMM_Parallel_Runner.API.Contracts;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;

namespace CMM_Parallel_Runner.API
{
    public class GrpcService : CMM_Parallel_Runner_Grpc_Service.CMM_Parallel_Runner_Grpc_ServiceBase
    {
        private readonly ICmmProcessor _cmmProcessor;

        private readonly Dictionary<string, Session> _sessions;

        public GrpcService(ICmmProcessor cmmProcessor)
        {
            _cmmProcessor = cmmProcessor;

            _sessions = new Dictionary<string, Session>();
        }

        public override async Task GrpcHandShaking(GrpcHandShakingRequest request, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
            Session session = null;
            string sessionId = GetSessionId(context);

            lock (_sessions)
            {
                if (_sessions.TryGetValue(sessionId, out session))
                {
                    throw new Exception($"The session Id {sessionId} already exists");
                }

                session = new Session(_cmmProcessor, responseStream, context);
                _sessions.Add(sessionId, session);
            }

            try
            {
                await session.Process();
            }
            finally
            {
                session.Dispose();

                lock (_sessions)
                {
                    _sessions.Remove(sessionId);
                }
            }

            return;
        }

        public override Task<GrpcCmmResult> GrpcDoCmmExport(GrpcCmmExportRequest request, ServerCallContext context)
        {
            try
            {
                Session session;
                string sessionId = GetSessionId(context);

                lock (_sessions)
                {
                    if (!_sessions.TryGetValue(sessionId, out session))
                    {
                        throw new Exception($"The session Id {sessionId} was not found");
                    }
                }

                GrpcCmmResult response = session.AddCmmExportRequest(request);

                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                return Task.FromResult(new GrpcCmmResult {Result = eGrpcExportResult.Fail, Message = e.ToString()});
            }
        }

        public override Task<GrpcCmmResult> GRpcStop(GrpcNone request, ServerCallContext context)
        {
            try
            {
                string sessionId = GetSessionId(context);

                lock (_sessions)
                {
                    if (!_sessions.TryGetValue(sessionId, out Session session))
                    {
                        throw new Exception($"The session Id {sessionId} was not found");
                    }

                    session.Stop();
                }

                return Task.FromResult(new GrpcCmmResult());
            }
            catch (Exception e)
            {
                return Task.FromResult(new GrpcCmmResult {Result = eGrpcExportResult.Fail, Message = e.ToString()});
            }
        }

        private static string GetSessionId(ServerCallContext context) => context.Peer;
    }
}
