using CMM_Parallel_Runner.GRpc.Classes;

namespace CMM_Parallel_Runner.API.Contracts
{
    public class CmmExportRequest
    {
        public CmmExportRequest(GrpcCmmExportRequest grpcCmmExportRequest)
        {
            Job = grpcCmmExportRequest.Job;
            Setup = grpcCmmExportRequest.Setup;
            Lot = grpcCmmExportRequest.Lot;
            WaferId = grpcCmmExportRequest.WaferId;
            ResultPath = grpcCmmExportRequest.ResultPath;
        }

        public string Job { get; }
        
        public string Setup { get; }
        
        public string Lot { get; }
        
        public string WaferId { get; }

        public string ResultPath { get; }
    }
}