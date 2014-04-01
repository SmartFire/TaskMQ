﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskUniversum.Assembly;
using TaskUniversum.Statistics;

namespace TaskUniversum
{
    public delegate void RestartApplication();
    public interface IBroker
    {
        void RegisterTempTask(Task.MetaTask mst, IBrokerModule module);
        RestartApplication resetBroker { get; }
        RestartApplication restartApp { get; }
        IEnumerable<KeyValuePair<string, IAssemblyStatus>> GetSourceStatuses();

        TaskQueue.RepresentedModel GetValidationModel(string MessageType, string ChannelName = null);
        bool PushMessage(TaskQueue.Providers.TaskMessage msg);

        ISourceManager GetSourceManager();
        StatisticContainer GetChannelsStatistic();
    }
}
