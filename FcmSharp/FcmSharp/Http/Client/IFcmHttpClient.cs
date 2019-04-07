// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FcmSharp.Http.Builder;

namespace FcmSharp.Http.Client
{
    public interface IFcmHttpClient : IDisposable
    {
        Task<TResponseType> SendAsync<TResponseType>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken);

        Task<TResponseType> SendAsync<TResponseType>(HttpRequestMessageBuilder builder, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task SendAsync(HttpRequestMessageBuilder builder, CancellationToken cancellationToken);

        Task SendAsync(HttpRequestMessageBuilder builder, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<TResponseType[]> SendBatchAsync<TResponseType>(HttpRequestMessageBuilder builder, CancellationToken cancellationToken);
    }
}