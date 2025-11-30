using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ServiceProcessing
{
    public interface IServiceProcessing
    {
        /// <summary>
        /// Starts asynchronous processing of a work item under the specified service processing name.
        /// </summary>
        /// <param name="workItem">A delegate that represents the work item to execute. The delegate receives a <see cref="CancellationToken"/>
        /// that is signaled when processing should be canceled. Cannot be null.</param>
        /// <param name="serviceProcessingName">The name that identifies the service processing context for this work item. Used to group or track
        /// processing operations.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task completes when the work item has
        /// finished processing or has been canceled.</returns>
        Task StartProcessing(Func<CancellationToken, Task> workItem, ServiceProcessingName serviceProcessingName);
    }
}
