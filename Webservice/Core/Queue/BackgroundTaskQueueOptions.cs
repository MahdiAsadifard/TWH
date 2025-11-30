using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Queue
{
    public class BackgroundTaskQueueOptions
    {
        public const string OptionName = "BackgroundTaskQueue";
        public int RetryEnqueueCount { get; set; }
        public int RetryQueueDelaySeconds { get; set; }
    }
}
