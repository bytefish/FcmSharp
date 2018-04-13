// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FcmSharp.Model.Topics
{
    public enum ConditionOperator { Or, And }

    public class TopicList
    {
        public readonly List<Topic> topics;
        public readonly ConditionOperator conditionOperator;

        public TopicList(List<Topic> topics, ConditionOperator conditionOperator = ConditionOperator.Or)
        {
            if (topics == null)
            {
                throw new ArgumentNullException("topics");
            }

            this.topics = topics;
            this.conditionOperator = conditionOperator;
        }

        public string GetTopicsCondition()
        {
            switch (conditionOperator)
            {
                case ConditionOperator.And:
                    return string.Join("&&", topics.Select(x => string.Format("'{0}' in topics", x.Name)));

                case ConditionOperator.Or:
                default:
                    return string.Join("||", topics.Select(x => string.Format("'{0}' in topics", x.Name)));
            }
        }
    }
}