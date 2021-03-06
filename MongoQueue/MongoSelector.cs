﻿using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoQueue
{
    public class MongoSelector
    {
        public static IMongoQuery GetQuery(TaskQueue.TQItemSelector selector)
        {
            List<IMongoQuery> qs = new List<IMongoQuery>();
            foreach (KeyValuePair<string, TaskQueue.TQItemSelectorParam> kv in selector.parameters)
            {
                switch (kv.Value.ValueSet)
                {
                    case TaskQueue.TQItemSelectorSet.Equals:
                        Type kvt = kv.Value.Value.GetType();
                        qs.Add(new QueryDocument(new Dictionary<string, object>() { { kv.Key, kv.Value.Value } }));
                        break;
                }
            }
            IMongoQuery q = Query.And(qs);
            return q;
        }
        public static SortByBuilder GetSort(TaskQueue.TQItemSelector selector)
        {
            SortByBuilder sb = new SortByBuilder();
            foreach (KeyValuePair<string, TaskQueue.TQItemSelectorParam> kv in selector.parameters)
            {
                switch (kv.Value.ValueSet)
                {
                    case TaskQueue.TQItemSelectorSet.Ascending:
                        sb = sb.Ascending(kv.Key);
                        break;
                    case TaskQueue.TQItemSelectorSet.Descending:
                        sb = sb.Descending(kv.Key);
                        break;
                }
            }
            return sb;
        }
        public static IndexKeysBuilder GetIndex(TaskQueue.TQItemSelector selector)
        {
            IndexKeysBuilder ikb = new IndexKeysBuilder();

            foreach (KeyValuePair<string, TaskQueue.TQItemSelectorParam> kv in selector.parameters)
            {
                switch (kv.Value.ValueSet)
                {
                    case TaskQueue.TQItemSelectorSet.Equals:
                        Type kvt = kv.Value.Value.GetType();
                        if (kvt == typeof(bool))
                        {
                            if ((bool)kv.Value.Value)
                            {
                                ikb = ikb.Ascending(kv.Key);
                            }
                            else
                            {
                                ikb = ikb.Descending(kv.Key);
                            }
                        }
                        else
                        {
                            ikb = ikb.Ascending(kv.Key);
                        }
                        break;
                }
            }
            foreach (KeyValuePair<string, TaskQueue.TQItemSelectorParam> kv in selector.parameters)
            {
                switch (kv.Value.ValueSet)
                {
                    case TaskQueue.TQItemSelectorSet.Ascending:
                        ikb = ikb.Ascending(kv.Key);
                        break;
                    case TaskQueue.TQItemSelectorSet.Descending:
                        ikb = ikb.Descending(kv.Key);
                        break;
                }
            }
            return ikb;
        }
    }
}
