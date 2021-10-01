/* Copyright (C) 2014-2017 Tal Aloni <tal.aloni.il@gmail.com>. All rights reserved.
 * 
 * You can redistribute this program and/or modify it under the terms of
 * the GNU Lesser Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version.
 */
using System;
using System.Text;
using Utilities;

namespace SMBLibrary.Authentication.NTLM
{
    /// <summary>
    /// NTLMv2_CLIENT_CHALLENGE
    /// </summary>
    public class NTLMv2ClientChallenge
    {
        public const int MinimumLength = 32;
        public const byte StructureVersion = 0x01;
        public static readonly DateTime EpochTime = DateTime.FromFileTimeUtc(0);

        public byte CurrentVersion;
        public byte MaximumSupportedVersion;
        public ushort Reserved1;
        public uint Reserved2;
        public DateTime TimeStamp;
        public uint Reserved3;
        public byte[] ClientChallenge; // 8-byte challenge generated by the client
        public KeyValuePairList<AVPairKey, byte[]> AVPairs;

        public NTLMv2ClientChallenge()
        {
        }

        public NTLMv2ClientChallenge(DateTime timeStamp, byte[] clientChallenge, string domainName, string computerName)
        {
            CurrentVersion = StructureVersion;
            MaximumSupportedVersion = StructureVersion;
            TimeStamp = timeStamp;
            ClientChallenge = clientChallenge;
            AVPairs = new KeyValuePairList<AVPairKey, byte[]>();
            AVPairs.Add(AVPairKey.NbDomainName, UnicodeEncoding.Unicode.GetBytes(domainName));
            AVPairs.Add(AVPairKey.NbComputerName, UnicodeEncoding.Unicode.GetBytes(computerName));
        }

        public NTLMv2ClientChallenge(DateTime timeStamp, byte[] clientChallenge, KeyValuePairList<AVPairKey, byte[]> targetInfo)
        {
            CurrentVersion = StructureVersion;
            MaximumSupportedVersion = StructureVersion;
            TimeStamp = timeStamp;
            ClientChallenge = clientChallenge;
            AVPairs = targetInfo;
        }

        public NTLMv2ClientChallenge(byte[] buffer) : this(buffer, 0)
        {
        }

        public NTLMv2ClientChallenge(byte[] buffer, int offset)
        {
            CurrentVersion = ByteReader.ReadByte(buffer, offset + 0);
            MaximumSupportedVersion = ByteReader.ReadByte(buffer, offset + 1);
            Reserved1 = LittleEndianConverter.ToUInt16(buffer, offset + 2);
            Reserved2 = LittleEndianConverter.ToUInt32(buffer, offset + 4);
            TimeStamp = FileTimeHelper.ReadFileTime(buffer, offset + 8);
            ClientChallenge = ByteReader.ReadBytes(buffer, offset + 16, 8);
            Reserved3 = LittleEndianConverter.ToUInt32(buffer, offset + 24);
            AVPairs = AVPairUtils.ReadAVPairSequence(buffer, offset + 28);
        }

        public byte[] GetBytes()
        {
            byte[] sequenceBytes = AVPairUtils.GetAVPairSequenceBytes(AVPairs);
            
            byte[] buffer = new byte[28 + sequenceBytes.Length];
            ByteWriter.WriteByte(buffer, 0, CurrentVersion);
            ByteWriter.WriteByte(buffer, 1, MaximumSupportedVersion);
            LittleEndianWriter.WriteUInt16(buffer, 2, Reserved1);
            LittleEndianWriter.WriteUInt32(buffer, 4, Reserved2);
            FileTimeHelper.WriteFileTime(buffer, 8, TimeStamp);
            ByteWriter.WriteBytes(buffer, 16, ClientChallenge, 8);
            LittleEndianWriter.WriteUInt32(buffer, 24, Reserved3);
            ByteWriter.WriteBytes(buffer, 28, sequenceBytes);
            return buffer;
        }

        /// <summary>
        /// [MS-NLMP] Page 60, Response key calculation algorithm:
        /// To create 'temp', 4 zero bytes will be appended to NTLMv2_CLIENT_CHALLENGE
        /// </summary>
        public byte[] GetBytesPadded()
        {
            return ByteUtils.Concatenate(GetBytes(), new byte[4]);
        }
    }
}
