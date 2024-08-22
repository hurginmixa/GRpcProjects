using CMM_Parallel_Runner.GRpc.Classes;

namespace CMM_Parallel_Runner.API.Contracts
{
    public class CreateFileInfo
    {
        public CreateFileInfo(string converterName, int converterId, string fileName)
        {
            ConverterName = converterName;
            ConverterId = converterId;
            FileName = fileName;
        }

        public string ConverterName { get; }
        
        public int ConverterId { get; }
        
        public string FileName { get; }

        public GrpcCreateFileInfo AsGrpc()
        {
            GrpcCreateFileInfo createFileInfo = new GrpcCreateFileInfo()
                {ConverterId = this.ConverterId, ConverterName = this.ConverterName, FileName = this.FileName};

            return createFileInfo;
        }
    }
}