using System;
using System.Threading;
using CMM_Parallel_Runner.API.Contracts;

namespace CMM_Parallel_Runner.Server
{
    internal class CmmProcessor : ICmmProcessor
    {
        public CmmExportResult DoCmmExport(CmmExportRequest request)
        {
            Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 2));

            return new CmmExportResult(true, string.Empty);
        }
    }
}
