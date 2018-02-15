// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Protocols;
using Microsoft.AspNetCore.Sockets;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.SignalR
{
    public static class SignalRSocketBuilderExtensions
    {
        public static IConnectionBuilder UseHub(this IConnectionBuilder socketBuilder, Type hubType)
        {
            if(!typeof(Hub).IsAssignableFrom(hubType))
            {
                throw new ArgumentException($"Hub type must be assignable to {typeof(Hub).FullName}", nameof(hubType));
            }

            var endpoint = (IHubEndPoint)socketBuilder.ApplicationServices.GetRequiredService(typeof(HubEndPoint<>).MakeGenericType(hubType));
            return socketBuilder.UseHub(endpoint);
        }

        public static IConnectionBuilder UseHub<THub>(this IConnectionBuilder socketBuilder) where THub : Hub
        {
            var endpoint = socketBuilder.ApplicationServices.GetRequiredService<HubEndPoint<THub>>();
            return socketBuilder.UseHub(endpoint);
        }

        private static IConnectionBuilder UseHub(this IConnectionBuilder socketBuilder, IHubEndPoint endpoint)
        {
            return socketBuilder.Run(connection => endpoint.OnConnectedAsync(connection));
        }
    }
}
