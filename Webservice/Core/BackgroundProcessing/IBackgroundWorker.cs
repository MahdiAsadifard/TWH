using System;
using System.Collections.Generic;
using System.Text;

namespace Core.BackgroundProcessing
{
    public interface IBackgroundWorker
    {
        /// <summary>
        /// Performs the work operation asynchronously, supporting cancellation through a token.
        /// </summary>
        /// <param name="cancellationToken">A token that can be used to request cancellation of the operation before it completes.</param>
        /// <returns>A task that represents the asynchronous work operation. The task completes when the operation finishes or is
        /// canceled.</returns>
        Task DoWork(CancellationToken cancellationToken);
    }
}
