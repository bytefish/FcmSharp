// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace FcmSharp.Serializer
{
    public interface IJsonSerializer
    {
        string SerializeObject(object value);
        
        TTargetType DeserializeObject<TTargetType>(string value);
    }
}