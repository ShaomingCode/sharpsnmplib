/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2010/12/5
 * Time: 10:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Lextm.SharpSnmpLib.Unit
{
    public class InformRequestPduTestFixture
    {
        [Fact]
        public void TestException()
        {
            Assert.Throws<ArgumentNullException>(() => new InformRequestPdu(0, new Span<byte>(new byte[] { 0 }), null));
            Assert.Throws<ArgumentNullException>(() => new InformRequestPdu(0, null, 0, null));
            Assert.Throws<ArgumentNullException>(() => new InformRequestPdu(0, new ObjectIdentifier("1.3.6.1"), 0, null));
#if !NETCOREAPP2_0
            Assert.Throws<ArgumentNullException>(() => new InformRequestPdu(null, new MemoryStream()));
#endif
            var pdu = new InformRequestPdu(0, new ObjectIdentifier("1.3.6.1"), 0, new List<Variable>());
            Assert.Throws<ArgumentNullException>(() => pdu.AppendBytesTo(null));
            Assert.Equal("INFORM request PDU: seq: 0; enterprise: 1.3.6.1; time stamp: 00:00:00; variable count: 0", pdu.ToString());
            Assert.Throws<NotSupportedException>(() => { var test = pdu.ErrorStatus; });
            Assert.Throws<NotSupportedException>(() => { var test = pdu.ErrorIndex; });
        }
    }
}
