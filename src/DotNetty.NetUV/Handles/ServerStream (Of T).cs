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

    public abstract class ServerStream<THandle> : StreamHandle<THandle>, IInternalServerStream
        where THandle : ServerStream<THandle>
    {
        internal const int DefaultBacklog = ServerStream.DefaultBacklog;

        internal static readonly uv_watcher_cb ConnectionCallback = (h, s) => OnConnectionCallback(h, s);

        private Action<THandle, Exception> _connectionHandler;

        internal ServerStream(
            LoopContext loop,
            uv_handle_type handleType,
            params object[] args)
            : base(loop, handleType, args)
        { }

        void IInternalServerStream.OnConnection(IInternalStreamHandle handle, Exception exception) => _connectionHandler((THandle)handle, exception);

        IInternalStreamHandle IInternalServerStream.NewStream() => NewStream();

        internal abstract IInternalStreamHandle NewStream();

        public void StreamListen(Action<THandle, Exception> onConnection, int backlog = DefaultBacklog)
        {
            if (onConnection is null) { ThrowHelper.ThrowArgumentNullException(ExceptionArgument.onConnection); }
            if ((uint)(backlog - 1) > SharedConstants.TooBigOrNegative) { ThrowHelper.ThrowArgumentException_Positive(backlog, ExceptionArgument.backlog); }

            Validate();
            _connectionHandler = onConnection;
            try
            {
                NativeMethods.StreamListen(InternalHandle, backlog);
#if DEBUG
                if (Log.DebugEnabled)
                {
                    Log.Debug("Stream {} {} listening, backlog = {}", HandleType, InternalHandle, backlog);
                }
#endif
            }
            catch
            {
                Dispose();
                throw;
            }
        }

        protected override void Close()
        {
            _connectionHandler = null;
            base.Close();
        }

        private static void OnConnectionCallback(IntPtr handle, int status)
        {
            var server = HandleContext.GetTarget<THandle>(handle);
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

                server._connectionHandler((THandle)client, error);
            }
            catch
            {
                client?.Dispose();
                throw;
            }
        }
    }
}
