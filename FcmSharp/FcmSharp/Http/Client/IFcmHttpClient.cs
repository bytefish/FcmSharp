// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FcmSharp.Http
{
    public interface IFcmHttpClient : IDisposable
    {
        Task PostAsync<TRequestType>(TRequestType request, CancellationToken cancellationToken);

        Task PostAsync<TRequestType>(TRequestType request, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<TResponseType> PostAsync<TRequestType, TResponseType>(TRequestType request, CancellationToken cancellationToken);

        Task<TResponseType> PostAsync<TRequestType, TResponseType>(TRequestType request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    }
}