﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetty.Codecs.Tests.Frame
{
    using System;
    using System.Linq;
    using System.Text;
    using DotNetty.Buffers;
    using DotNetty.Transport.Channels.Embedded;
    using Xunit;

    public class LengthFieldPrepender2Test
    {
        readonly IByteBuffer msg;

        public LengthFieldPrepender2Test()
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            byte[] charBytes = iso.GetBytes("A");
            this.msg = Unpooled.CopiedBuffer(charBytes);
        }

        [Fact]
        public void TestPrependLength()
        {
            var ch = new EmbeddedChannel(new LengthFieldPrepender2(4));
            ch.WriteOutbound(this.msg);
            var buf = ch.ReadOutbound<IByteBuffer>();
            Assert.Equal(5, buf.ReadableBytes);
            Assert.Equal(this.msg.ReadableBytes, buf.ReadInt());
            Assert.Equal('A', (char)buf.ReadByte());
            buf.Release();
        }

        [Fact]
        public void TestPrependLengthIncludesLengthFieldLength()
        {
            var ch = new EmbeddedChannel(new LengthFieldPrepender2(4, true));
            ch.WriteOutbound(this.msg);
            var buf = ch.ReadOutbound<IByteBuffer>();
            Assert.Equal(5, buf.ReadableBytes);
            Assert.Equal(5, buf.ReadInt());
            Assert.Equal('A', (char)buf.ReadByte());
            buf.Release();
        }

        [Fact]
        public void TestPrependAdjustedLength()
        {
            var ch = new EmbeddedChannel(new LengthFieldPrepender2(4, -1));
            ch.WriteOutbound(this.msg);
            var buf = ch.ReadOutbound<IByteBuffer>();
            Assert.Equal(5, buf.ReadableBytes);
            Assert.Equal(this.msg.ReadableBytes - 1, buf.ReadInt());
            Assert.Equal('A', (char)buf.ReadByte());
            buf.Release();
        }

        [Fact]
        public void TestPrependAdjustedLengthLessThanZero()
        {
            var ch = new EmbeddedChannel(new LengthFieldPrepender2(4, -2));
            var ex = Assert.Throws<AggregateException>(() =>
            {
                ch.WriteOutbound(this.msg);
                Assert.True(false, typeof(EncoderException).Name + " must be raised.");
            });

            Assert.IsType<EncoderException>(ex.InnerExceptions.Single());
        }

        //[Fact]
        //public void TestPrependLengthInLittleEndian()
        //{
        //    var ch = new EmbeddedChannel(new LengthFieldPrepender2(ByteOrder.LittleEndian, 4, 0, false));
        //    ch.WriteOutbound(this.msg);
        //    var buf = ch.ReadOutbound<IByteBuffer>();
        //    Assert.Equal(5, buf.ReadableBytes);
        //    var writtenBytes = new byte[buf.ReadableBytes];
        //    buf.GetBytes(0, writtenBytes);
        //    Assert.Equal(1, writtenBytes[0]);
        //    Assert.Equal(0, writtenBytes[1]);
        //    Assert.Equal(0, writtenBytes[2]);
        //    Assert.Equal(0, writtenBytes[3]);
        //    Assert.Equal('A', (char)buf.GetByte(4));
        //    buf.Release();

        //    Assert.False(ch.Finish(), "The channel must have been completely read");
        //}
    }
}