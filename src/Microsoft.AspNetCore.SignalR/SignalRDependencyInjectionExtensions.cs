// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.SignalR;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SignalRDependencyInjectionExtensions
    {
        public static ISignalRBuilder AddSignalR(this IServiceCollection services)
        {
            services.AddSockets();
            return services.AddSignalRCore()
                .AddJsonProtocol();
        }

        public static ISignalRBuilder AddHubOptions<THub>(this ISignalRBuilder signalrBuilder, Action<HubOptions<THub>> options) where THub : Hub
        {
            signalrBuilder.Services.Configure(options);
            return signalrBuilder;
        }
    }
}
