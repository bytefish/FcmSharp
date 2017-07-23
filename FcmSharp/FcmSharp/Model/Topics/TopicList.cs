// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace FcmSharp.Model.Topics
{
    public class TopicList
    {
        public readonly List<Topic> topics;

        public TopicList(List<Topic> topics)
        {
            if (topics == null)
            {
                throw new ArgumentNullException("topics");
            }
            this.topics = topics;
        }

        public string GetTopicsCondition()
        {
            return string.Join("||", topics.Select(x => string.Format("'{0}' in topics", x.Name)));
        }
    }
}