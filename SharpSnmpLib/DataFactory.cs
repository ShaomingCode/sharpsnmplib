// Data factory type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/30
 * Time: 19:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Globalization;
using System.IO;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Factory that creates <see cref="ISnmpData"/> instances.
    /// </summary>
    public static class DataFactory
    {
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            return CreateSnmpData(buffer, 0, buffer.Length);
        }

#if NETCOREAPP2_0
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="type">Type code.</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(Span<byte> span, int start, out int next)
        {
            var type = span[start];
            var offset = start + 1;
            var container = span.Slice(offset);
            var Item1 = 0;
            var Item2 = container.ReadPayloadLength(out Item1);
            var stream = container.Slice(Item2.Length, Item1);
            next = 1 + start + Item2.Length + Item1;
            try
            {
                switch ((SnmpType)type)
                {
                    case SnmpType.Counter32:
                        return new Counter32(Item1, Item2, stream);
                    case SnmpType.Counter64:
                        return new Counter64(Item1, Item2, stream);
                    case SnmpType.Gauge32:
                        return new Gauge32(Item1, Item2, stream);
                    case SnmpType.ObjectIdentifier:
                        return new ObjectIdentifier(Item1, Item2, stream);
                    case SnmpType.Null:
                        return new Null(Item1, Item2, stream);
                    case SnmpType.NoSuchInstance:
                        return new NoSuchInstance(Item1, Item2, stream);
                    case SnmpType.NoSuchObject:
                        return new NoSuchObject(Item1, Item2, stream);
                    case SnmpType.EndOfMibView:
                        return new EndOfMibView(Item1, Item2, stream);
                    case SnmpType.Integer32:
                        return new Integer32(Item1, Item2, stream);
                    case SnmpType.OctetString:
                        return new OctetString(Item1, Item2, stream);
                    case SnmpType.IPAddress:
                        return new IP(Item1, Item2, stream);
                    case SnmpType.TimeTicks:
                        return new TimeTicks(Item1, Item2, stream);
                    case SnmpType.Sequence:
                        return new Sequence(Item1, Item2, stream);
                    case SnmpType.TrapV1Pdu:
                        return new TrapV1Pdu(Item1, Item2, stream);
                    case SnmpType.TrapV2Pdu:
                        return new TrapV2Pdu(Item1, Item2, stream);
                    case SnmpType.GetRequestPdu:
                        return new GetRequestPdu(Item1, Item2, stream);
                    case SnmpType.ResponsePdu:
                        return new ResponsePdu(Item1, Item2, stream);
                    case SnmpType.GetBulkRequestPdu:
                        return new GetBulkRequestPdu(Item1, Item2, stream);
                    case SnmpType.GetNextRequestPdu:
                        return new GetNextRequestPdu(Item1, Item2, stream);
                    case SnmpType.SetRequestPdu:
                        return new SetRequestPdu(Item1, Item2, stream);
                    case SnmpType.InformRequestPdu:
                        return new InformRequestPdu(Item1, Item2, stream);
                    case SnmpType.ReportPdu:
                        return new ReportPdu(Item1, Item2, stream);
                    case SnmpType.Opaque:
                        return new Opaque(Item1, Item2, stream);
                    case SnmpType.EndMarker:
                        return null;
                    case SnmpType.Unsigned32:
                        // IMPORTANT: return Gauge32 for Unsigned32 case as workaround of RFC 1442 time entities.
                        return new Gauge32(Item1, Item2, stream);
                    default:
                        throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "unsupported data type: {0}", (SnmpType)type));
                }
            }
            catch (Exception ex)
            {
                if (ex is SnmpException)
                {
                    throw;
                }

                throw new SnmpException("data construction exception", ex);
            }
        }

        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(byte[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var m = new Span<byte>(buffer, index, count);
            var next = 0;
            return CreateSnmpData(m, 0, out next);
        }
#else 
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="type">Type code.</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(int type, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var length = stream.ReadPayloadLength();
            try
            {
                switch ((SnmpType)type)
                {
                    case SnmpType.Counter32:
                        return new Counter32(length, stream);
                    case SnmpType.Counter64:
                        return new Counter64(length, stream);
                    case SnmpType.Gauge32:
                        return new Gauge32(length, stream);
                    case SnmpType.ObjectIdentifier:
                        return new ObjectIdentifier(length, stream);
                    case SnmpType.Null:
                        return new Null(length, stream);
                    case SnmpType.NoSuchInstance:
                        return new NoSuchInstance(length, stream);
                    case SnmpType.NoSuchObject:
                        return new NoSuchObject(length, stream);
                    case SnmpType.EndOfMibView:
                        return new EndOfMibView(length, stream);
                    case SnmpType.Integer32:
                        return new Integer32(length, stream);
                    case SnmpType.OctetString:
                        return new OctetString(length, stream);
                    case SnmpType.IPAddress:
                        return new IP(length, stream);
                    case SnmpType.TimeTicks:
                        return new TimeTicks(length, stream);
                    case SnmpType.Sequence:
                        return new Sequence(length, stream);
                    case SnmpType.TrapV1Pdu:
                        return new TrapV1Pdu(length, stream);
                    case SnmpType.TrapV2Pdu:
                        return new TrapV2Pdu(length, stream);
                    case SnmpType.GetRequestPdu:
                        return new GetRequestPdu(length, stream);
                    case SnmpType.ResponsePdu:
                        return new ResponsePdu(length, stream);
                    case SnmpType.GetBulkRequestPdu:
                        return new GetBulkRequestPdu(length, stream);
                    case SnmpType.GetNextRequestPdu:
                        return new GetNextRequestPdu(length, stream);
                    case SnmpType.SetRequestPdu:
                        return new SetRequestPdu(length, stream);
                    case SnmpType.InformRequestPdu:
                        return new InformRequestPdu(length, stream);
                    case SnmpType.ReportPdu:
                        return new ReportPdu(length, stream);
                    case SnmpType.Opaque:
                        return new Opaque(length, stream);
                    case SnmpType.EndMarker:
                        return null;
                    case SnmpType.Unsigned32:
                        // IMPORTANT: return Gauge32 for Unsigned32 case as workaround of RFC 1442 time entities.
                        return new Gauge32(length, stream);
                    default:
                        throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "unsupported data type: {0}", (SnmpType)type));
                }
            }
            catch (Exception ex)
            {
                if (ex is SnmpException)
                {
                    throw;
                }

                throw new SnmpException("data construction exception", ex);
            }
        }

        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from buffer.
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="index">Index</param>
        /// <param name="count">Count</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(byte[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            using (var m = new MemoryStream(buffer, index, count, false))
            {
                return CreateSnmpData(m);
            }
        }
        
        /// <summary>
        /// Creates an <see cref="ISnmpData"/> instance from stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns></returns>
        public static ISnmpData CreateSnmpData(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return CreateSnmpData(stream.ReadByte(), stream);
        }
#endif
    }
}
