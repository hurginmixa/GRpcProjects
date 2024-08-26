using System;
using System.Threading.Tasks;
using CMM_Parallel_Runner.API.Contracts;
using CMM_Parallel_Runner.GRpc.Classes;
using Grpc.Core;

namespace CMM_Parallel_Runner.API
{
    public class GrpcService : CMM_Parallel_Runner_Grpc_Service.CMM_Parallel_Runner_Grpc_ServiceBase
    {
        private readonly ICmmProcessor _cmmProcessor;
        private readonly SessionDictionary _sessions;

        public GrpcService(ICmmProcessor cmmProcessor)
        {
            _cmmProcessor = cmmProcessor;

            _sessions = new SessionDictionary();
        }

        public SessionDictionary Sessions => _sessions;

        public override async Task GrpcHandShaking(GrpcHandShakingRequest request, IServerStreamWriter<GrpcExportResult> responseStream, ServerCallContext context)
        {
            string sessionId = GetSessionId(context);

            Session session = _sessions.AddNewSession(sessionId, _cmmProcessor, responseStream, context);

            try
            {
                await session.Process();
            }
            finally
            {
                _sessions.Remove(sessionId);
            }
        }

        public override Task<GrpcCmmResult> GrpcDoCmmExport(GrpcCmmExportRequest request, ServerCallContext context)
        {
            try
            {
                string sessionId = GetSessionId(context);

                Session session = _sessions.GetSession(sessionId);

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

                Session session = _sessions.GetSession(sessionId);

                session.Stop();

                return Task.FromResult(new GrpcCmmResult {Result = eGrpcExportResult.Ok});
            }
            catch (Exception e)
            {
                return Task.FromResult(new GrpcCmmResult {Result = eGrpcExportResult.Fail, Message = e.ToString()});
            }
        }

        private static string GetSessionId(ServerCallContext context) => context.Peer;
    }
}
