using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Queue
{
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Enqueues a background work item for asynchronous execution.
        /// </summary>
        /// <remarks>The work item will be executed on a background thread. The returned <see
        /// cref="ValueTask"/> completes when the work item has been successfully enqueued, not when it has finished
        /// executing. Multiple calls to this method may be processed concurrently.</remarks>
        /// <param name="workItem">A delegate representing the work to execute. The delegate receives a <see cref="CancellationToken"/> that is
        /// triggered if the operation is cancelled. Cannot be null.</param>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous enqueue operation.</returns>
        ValueTask<ValueTask> EnqueueAsync(Func<CancellationToken, Task> workItem);

        /// <summary>
        /// Asynchronously dequeues the next available work item from the queue.
        /// </summary>
        /// <remarks>The returned delegate should be invoked to process the work item. Multiple calls to
        /// this method may wait until items are available in the queue. This method is thread-safe and can be called
        /// concurrently from multiple threads.</remarks>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the dequeue operation.</param>
        /// <returns>A value task that represents the asynchronous operation. The result is a delegate that, when invoked with a
        /// cancellation token, executes the dequeued work item. If the queue is empty, the task may complete when an
        /// item becomes available or be canceled if the token is triggered.</returns>
        ValueTask<Func<CancellationToken, Task>> DequeuAsync(CancellationToken cancellationToken);
    }
}
