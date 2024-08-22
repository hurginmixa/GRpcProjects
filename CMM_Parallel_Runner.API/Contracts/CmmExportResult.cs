using System;

namespace CMM_Parallel_Runner.API.Contracts
{
    public class CmmExportResult
    {
        public CmmExportResult(bool success, string errorString)
        {
            Success = success;
            ErrorString = errorString;
            CreateFileInfos = Array.Empty<CreateFileInfo>();
        }

        public bool Success { get; }

        public string ErrorString { get; }

        public CreateFileInfo[] CreateFileInfos { get; }
    }
}