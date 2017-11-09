﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Codecs.Http.WebSockets.Extensions.Compression
{
    class PerFrameDeflateDecoder : DeflateDecoder
    {
        public PerFrameDeflateDecoder(bool noContext)
            : base(noContext)
        {
        }

        public override bool TryAcceptInboundMessage(object msg, out WebSocketFrame frame)
        {
            switch (msg)
            {
                case TextWebSocketFrame textFrame when (textFrame.Rsv & WebSocketRsv.Rsv1) > 0:
                    frame = textFrame;
                    return true;
                case BinaryWebSocketFrame binFrame when (binFrame.Rsv & WebSocketRsv.Rsv1) > 0:
                    frame = binFrame;
                    return true;
                case ContinuationWebSocketFrame conFrame when (conFrame.Rsv & WebSocketRsv.Rsv1) > 0:
                    frame = conFrame;
                    return true;
                default:
                    frame = null;
                    return false;
            }
            //return (msg is TextWebSocketFrame || msg is BinaryWebSocketFrame || msg is ContinuationWebSocketFrame)
            //&& (((WebSocketFrame)msg).Rsv & WebSocketRsv.Rsv1) > 0;
        }

        protected override int NewRsv(WebSocketFrame msg) => msg.Rsv ^ WebSocketRsv.Rsv1;

        protected override bool AppendFrameTail(WebSocketFrame msg) => true;
    }
}