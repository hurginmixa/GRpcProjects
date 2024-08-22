namespace CMM_Parallel_Runner.API.Contracts
{
    public interface ICmmProcessor
    {
        CmmExportResult DoCmmExport(CmmExportRequest request);
    }
}