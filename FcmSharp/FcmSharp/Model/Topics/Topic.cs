// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Model.Topics
{
    public class Topic
    {
        public readonly string Name;

        public Topic(string name)
        {
            Name = name;
        }

        public string getTopicPath()
        {
            return string.Format("/{0}/{1}", "topics", Name);
        }
    }
}