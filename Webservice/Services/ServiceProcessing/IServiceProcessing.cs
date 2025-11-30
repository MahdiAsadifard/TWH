using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ServiceProcessing
{
    public interface IServiceProcessing
    {
        Task StartProcessing(Func<CancellationToken, Task> workItem);
    }
}
