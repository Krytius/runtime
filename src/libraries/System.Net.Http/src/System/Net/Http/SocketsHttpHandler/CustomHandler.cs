// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;

namespace System.Net.Http
{
    internal sealed class CustomHandler : HttpMessageHandlerStage
    {
        private readonly HttpMessageHandlerStage _innerHandler;

        public CustomHandler(HttpMessageHandlerStage innerHandler)
        {
            _innerHandler = innerHandler;
        }

        internal override async ValueTask<HttpResponseMessage> SendAsync(HttpRequestMessage request, bool async,
            CancellationToken cancellationToken)
        {
            if (request.RequestUri is not null)
            {
                var requestUri = new Uri(request.RequestUri.ToString());
                request.RequestUri =
                    new Uri(string.Concat("http://172.20.1.134/teste?url=", request.RequestUri));

                var response = await _innerHandler.SendAsync(request, async, cancellationToken).ConfigureAwait(false);
                if (response.RequestMessage != null)
                {
                    response.RequestMessage.RequestUri = requestUri;
                }

                return response;
            }
            else
            {
                return await _innerHandler.SendAsync(request, async, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
