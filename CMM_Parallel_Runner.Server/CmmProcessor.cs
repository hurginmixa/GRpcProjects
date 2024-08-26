using System;
using System.Threading;
using CMM_Parallel_Runner.API.Contracts;

namespace CMM_Parallel_Runner.Server
{
    internal class CmmProcessor : ICmmProcessor
    {
        private readonly object _sync = new object();

        public CmmExportResult DoCmmExport(CmmExportRequest request)
        {
            lock (_sync)
            {
                Thread.Sleep(new TimeSpan(hours: 0, minutes: 0, seconds: 2));

                return new CmmExportResult(true, string.Empty);
            }
        }
    }
}
