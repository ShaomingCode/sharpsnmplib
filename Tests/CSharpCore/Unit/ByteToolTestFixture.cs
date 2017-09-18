/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/8/3
 * Time: 10:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

#pragma warning disable 1591, 0618
namespace Lextm.SharpSnmpLib.Unit
{
    public class ByteToolTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => ByteTool.GetRawBytes(null, true));
            Assert.Throws<ArgumentNullException>(() => ByteTool.ConvertDecimal(null));
            Assert.Throws<ArgumentNullException>(() => ByteTool.Convert((byte[])null));
            Assert.Throws<ArgumentNullException>(() => ByteTool.ParseItems(null));
            Assert.Throws<ArgumentException>(() => ByteTool.ParseItems((ISnmpData)null));
            Assert.Throws<ArgumentNullException>(() => ByteTool.ParseItems((IEnumerable<ISnmpData>)null));
            Assert.Throws<ArgumentNullException>(() => ByteTool.Convert((string)null));
            Assert.Throws<ArgumentException>(() => ByteTool.Convert("**"));
            Assert.Throws<ArgumentException>(() => ByteTool.Convert("8AB"));
            Assert.Throws<ArgumentException>(() => (-1).WritePayloadLength());
            Assert.Throws<ArgumentNullException>(() => ByteTool.PackMessage(null, VersionCode.V3, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => ByteTool.PackMessage(new byte[0], VersionCode.V3, null, null, null));
            Assert.Throws<ArgumentNullException>(
                () => ByteTool.PackMessage(new byte[0], VersionCode.V3, new Header(500), null, null));
            Assert.Throws<ArgumentNullException>(
                () => ByteTool.PackMessage(new byte[0], VersionCode.V3, new Header(500), SecurityParameters.Create(new OctetString("test")), null));
        }

        [Fact]
        public void TestConvertDecimal()
        {
            byte[] b = ByteTool.ConvertDecimal(" 16 18 ");
            Assert.Equal(new byte[] { 0x10, 0x12 }, b);
        }

#if NETCOREAPP2_0
        [Fact]
        public void TestReadShortLength()
        {
            var m = new Span<byte>(new[] { (byte)0x66 });
            var Item1 = 0;
            var Item2 = m.ReadPayloadLength(out Item1);
            Assert.Equal(102, Item1);
            Assert.Equal(new byte[] { 0x66 }, Item2.ToArray());
        }
#else
        [Fact]
        public void TestReadShortLength()
        {
            MemoryStream m = new MemoryStream();
            m.WriteByte(0x66);
            m.Flush();
            m.Position = 0;
            var result = m.ReadPayloadLength();
            Assert.Equal(102, result.Item1);
            Assert.Equal(new byte[] { 0x66 }, result.Item2);
        }
#endif

        [Fact]
        public void TestWriteShortLength()
        {
            const int length = 102;
            const byte expect = 0x66;
            var array = length.WritePayloadLength();
            Assert.Equal(1, array.Length);
            Assert.Equal(expect, array[0]);
        }

#if NETCOREAPP2_0
        [Fact]
        public void TestReadLongLength()
        {
            byte[] expected = new byte[] { 0x83, 0x73, 0x59, 0xB5 };
            var m = new Span<byte>(expected);
            var Item1 = 0;
            m.ReadPayloadLength(out Item1);
            Assert.Equal(7559605, Item1);
        }
#else
        [Fact]
        public void TestReadLongLength()
        {
            byte[] expected = new byte[] { 0x83, 0x73, 0x59, 0xB5 };
            MemoryStream m = new MemoryStream();
            m.Write(expected, 0, 4);
            m.Flush();
            m.Position = 0;
            Assert.Equal(7559605, m.ReadPayloadLength().Item1);
        }
#endif
        [Fact]
        public void TestWriteLongLength()
        {
            const int length = 7559605;
            byte[] expected = new byte[] { 0x83, 0x73, 0x59, 0xB5 };
            MemoryStream m = new MemoryStream();
            var array = length.WritePayloadLength();
            Assert.Equal(expected, array);
        }
    }
}
#pragma warning restore 1591, 0618
