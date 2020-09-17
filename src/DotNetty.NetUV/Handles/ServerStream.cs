/*
 * Copyright (c) Johnny Z. All rights reserved.
 *
 *   https://github.com/StormHub/NetUV
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 *
 * Copyright (c) 2020 The Dotnetty-Span-Fork Project (cuteant@outlook.com)
 *
 *   https://github.com/cuteant/dotnetty-span-fork
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 */

namespace DotNetty.NetUV.Handles
{
    using System;
    using DotNetty.NetUV.Native;

    internal static class ServerStream
    {
        internal const int DefaultBacklog = 128;

        internal static readonly uv_watcher_cb ConnectionCallback = (h, s) => OnConnectionCallback(h, s);

        private static void OnConnectionCallback(IntPtr handle, int status)
        {
            var server = HandleContext.GetTarget<IInternalServerStream>(handle);
            if (server is null) { return; }

            IInternalStreamHandle client = null;
            Exception error = null;
            try
            {
                if ((uint)status > SharedConstants.TooBigOrNegative) // < 0
                {
                    error = NativeMethods.CreateError((uv_err_code)status);
                }
                else
                {
                    client = server.NewStream();
                }

                server.OnConnection(client, error);
            }
            catch
            {
                client?.Dispose();
                throw;
            }
        }
    }
}
