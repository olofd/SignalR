// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Protocols;
using Microsoft.AspNetCore.Sockets;

namespace Microsoft.AspNetCore.SignalR
{
    public class HubRouteBuilder
    {
        private readonly SocketRouteBuilder _routes;

        public HubRouteBuilder(SocketRouteBuilder routes)
        {
            _routes = routes;
        }

        public void MapHub(string path, Type hubType)
        {
            MapHub(new PathString(path), hubType, socketOptions: null);
        }

        public void MapHub(PathString path, Type hubType)
        {
            MapHub(path, hubType, socketOptions: null);
        }

        public void MapHub(PathString path, Type hubType, Action<HttpSocketOptions> socketOptions)
        {
            MapHubCore(path, hubType, socketOptions, builder => builder.UseHub(hubType));
        }

        public void MapHub<THub>(string path) where THub : Hub
        {
            MapHub<THub>(new PathString(path), socketOptions: null);
        }

        public void MapHub<THub>(PathString path) where THub : Hub
        {
            MapHub<THub>(path, socketOptions: null);
        }

        public void MapHub<THub>(PathString path, Action<HttpSocketOptions> socketOptions) where THub : Hub
        {
            MapHubCore(path, typeof(THub), socketOptions, builder => builder.UseHub<THub>());
        }

        private void MapHubCore(PathString path, Type hubType, Action<HttpSocketOptions> socketOptions, Action<IConnectionBuilder> socketBuilder)
        {
            // find auth attributes
            var authorizeAttributes = hubType.GetCustomAttributes<AuthorizeAttribute>(inherit: true);
            var options = new HttpSocketOptions();
            foreach (var attribute in authorizeAttributes)
            {
                options.AuthorizationData.Add(attribute);
            }
            socketOptions?.Invoke(options);

            _routes.MapSocket(path, options, socketBuilder);
        }
    }
}
