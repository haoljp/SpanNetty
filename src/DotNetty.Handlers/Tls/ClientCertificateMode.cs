﻿/*
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

namespace DotNetty.Handlers.Tls
{
    /// <summary>Describes the client certificate requirements for a HTTPS connection.</summary>
    public enum ClientCertificateMode
    {
        /// <summary>A client certificate is not required and will not be requested from clients.</summary>
        NoCertificate,

        /// <summary>A client certificate will be requested; however,
        /// authentication will not fail if a certificate is not provided by the client.</summary>
        AllowCertificate,

        /// <summary>A client certificate will be requested, and the client must provide a valid certificate for authentication to succeed.</summary>
        RequireCertificate
    }
}
