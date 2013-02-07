﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskBroker
{
    public class QueueClassificator
    {
        public QueueClassificator()
        {
            addQueue(new TaskQueue.Providers.MemQueue());
        }
        // queues
        void addQueue(TaskQueue.ITQueue q)
        {
            QueueList.Add(q.QueueType, q);
        }

        public Dictionary<string, TaskQueue.ITQueue> QueueList = new Dictionary<string, TaskQueue.ITQueue>();
    }
}
