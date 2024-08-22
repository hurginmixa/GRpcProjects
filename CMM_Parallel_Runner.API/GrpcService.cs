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
        private readonly CancellationToken _cancellationToken;

        private readonly Dictionary<string, Session> _sessions;

        public GrpcService(ICmmProcessor cmmProcessor, CancellationTokenSource cancellationTokenSource)
        {
            _cmmProcessor = cmmProcessor;

            _cancellationToken = cancellationTokenSource.Token;

            _sessions = new Dictionary<string, Session>();
        }

        public override async Task HandShaking(GrpcNone request, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
            Session session = null;

            try
            {
                lock (_sessions)
                {
                    string sessionId = GetSessionId(context);

                    if(!_sessions.TryGetValue(sessionId, out session))
                    {
                        session = new Session(responseStream, _cmmProcessor);
                        _sessions.Add(sessionId, session);
                    }
                }

                await session.Process(_cancellationToken);
            }
            catch (Exception)
            {
                session?.Dispose();
                throw;
            }
        }

        public override Task<GrpcCmmExportResponse> DoCmmExport(GrpcCmmExportRequest request, ServerCallContext context)
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

                GrpcCmmExportResponse response = session.AddCmmExportRequest(request);

                return Task.FromResult(response);
            }
            catch (Exception e)
            {
                return Task.FromResult(new GrpcCmmExportResponse {Result = eGrpcExportResult.Fail, Message = e.ToString()});
            }
        }

        private static string GetSessionId(ServerCallContext context) => context.Host;
    }
}
