/*
 * Copyright 2012 The Netty Project
 *
 * The Netty Project licenses this file to you under the Apache License,
 * version 2.0 (the "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at:
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 * Copyright (c) 2020 The Dotnetty-Span-Fork Project (cuteant@outlook.com) All rights reserved.
 *
 *   https://github.com/cuteant/dotnetty-span-fork
 *
 * Licensed under the MIT license. See LICENSE file in the project root for full license information.
 */

using System;
using System.Runtime.CompilerServices;
using DotNetty.Buffers;
using DotNetty.Common.Concurrency;
using DotNetty.Common.Internal.Logging;
using DotNetty.NetUV.Handles;
using DotNetty.NetUV.Native;
using DotNetty.NetUV.Requests;

namespace DotNetty.NetUV
{
    internal static partial class NetUVLoggingExtensions
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void LoopThreadFinished(this IInternalLogger logger, XThread thread)
        {
            logger.Info("Loop {}: thread finished.", thread.Name);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void LoopDisposed(this IInternalLogger logger, XThread thread)
        {
            logger.Info("{}:disposed.", thread.Name);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void LoopDisposing(this IInternalLogger logger, IDisposable handle)
        {
            logger.Debug("Disposing {}", handle.GetType());
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void LoopReleaseError(this IInternalLogger logger, XThread thread, Exception ex)
        {
            logger.Warn("{}:release error {}", thread.Name, ex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void LoopRunDefaultError(this IInternalLogger logger, XThread thread, Exception ex)
        {
            logger.Error("Loop {}:run default error.", thread.Name, ex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void LoopDisposeError(this IInternalLogger logger, IDisposable handle, Exception ex)
        {
            logger.Warn("{} dispose error {}", handle.GetType(), ex);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void ShuttingDownLoopError(this IInternalLogger logger, Exception ex)
        {
            logger.Error("{}: shutting down loop error", ex);
        }
    }
}
