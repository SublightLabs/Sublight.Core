// ==++==
// 
//   Copyright (c) 2014 Sublight Labs (http://www.sublight.me) All rights reserved.
// 
// ==--==
/*============================================================
**
** Class:  HashUtility
**
**
** Purpose: Implementation of new hashing algorithm which
** is used in Sublight 4.5 and newer. New algorithm is
** much faster to calculate than in previous versions.
**
===========================================================*/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using Sublight.Core.Types;

namespace Sublight.Core
{
    public class HashUtility
    {
        /// <summary>
        /// Main method used for hash calculation.
        /// </summary>
        /// <param name="videoPath">A relative or absolute path for the video file which will be used for hash calculation.</param>
        /// <returns>40 byte (320-bit) video hash value wrapped in Result&lt;string&gt; class. If there are any problems Result.Status does not equal Success.</returns>
        public static async Task<Result<string>> CalculateHashAsync(string videoPath)
        {
            if (videoPath == null)
            {
                return Result<string>.CreateException(new ArgumentNullException("videoPath"));
            }
            Contract.EndContractBlock();

            var file = await FileSystem.Current.GetFileFromPathAsync(videoPath).ConfigureAwait(false);
            if (file == null)
            {
                return Result<string>.CreateError(string.Format("Could not open file: '{0}'", videoPath));
            }

            using (var fs = await file.OpenAsync(FileAccess.Read).ConfigureAwait(false))
            {
                return await CalculateHashAsync(fs).ConfigureAwait(false); ;
            }
        }

        public static async Task<Result<string>> CalculateHashAsync(Stream videoFileStream)
        {
            if (videoFileStream == null)
            {
                return Result<string>.CreateException(new ArgumentNullException("videoFileStream"));
            }
            //Contract.Ensures(Contract.Result<String>() != null && Contract.Result<String>().Length == 40);
            Contract.EndContractBlock();

            const int DATA_SIZE = 128 * 1024; //128 K

            long fileLength = videoFileStream.Length;
            if (fileLength < DATA_SIZE)
            {
                return Result<string>.CreateError(string.Format("Input file must be at least {0} B", DATA_SIZE));
            }

            if (!videoFileStream.CanSeek)
            {
                return Result<string>.CreateError("Stream does not support seeking");
            }

            byte[] bufferBegin = new byte[DATA_SIZE];
            videoFileStream.Seek(0, SeekOrigin.Begin);
            int fsRead = await videoFileStream.ReadAsync(bufferBegin, 0, bufferBegin.Length).ConfigureAwait(false); ;
            if (fsRead != bufferBegin.Length)
            {
                return Result<string>.CreateError("Error reading file");
            }

            byte[] bufferMiddle = new byte[DATA_SIZE];
            videoFileStream.Seek(fileLength / 2, SeekOrigin.Begin);
            await videoFileStream.ReadAsync(bufferMiddle, 0, bufferMiddle.Length).ConfigureAwait(false); ;

            byte[] bufferEnd = new byte[DATA_SIZE];
            videoFileStream.Seek(fileLength - DATA_SIZE, SeekOrigin.Begin);
            await videoFileStream.ReadAsync(bufferEnd, 0, bufferEnd.Length).ConfigureAwait(false); ;

            try
            {
                return Result<string>.CreateSuccess(ComputeHash2OrThrowException(fileLength, bufferBegin, bufferMiddle, bufferEnd));
            }
            catch (Exception ex)
            {
                return Result<string>.CreateException(ex);
            }
        }

        protected static string ComputeHash2OrThrowException(long fileLength, byte[] dataBegin, byte[] dataMiddle, byte[] dataEnd)
        {
            const byte TYPE_HASH2_NORMAL = 2;

            int checksumBegin = ComputeAddler32(dataBegin, 0, dataBegin.Length);
            int checksumMiddle = ComputeAddler32(dataMiddle, 0, dataMiddle.Length);
            int checksumEnd = ComputeAddler32(dataEnd, 0, dataEnd.Length);

            var hashBuffer = new List<byte>(20);

            hashBuffer.Insert(0, TYPE_HASH2_NORMAL);
            hashBuffer.InsertRange(1, GetLastBytes(Invert(BitConverter.GetBytes(fileLength)), 6));
            hashBuffer.InsertRange(7, BitConverter.GetBytes(checksumBegin));
            hashBuffer.InsertRange(11, BitConverter.GetBytes(checksumMiddle));
            hashBuffer.InsertRange(15, BitConverter.GetBytes(checksumEnd));

            int sum = 0;
            for (int i = 0; i < hashBuffer.Count; i++)
            {
                sum += hashBuffer[i];
            }
            hashBuffer.Insert(19, Convert.ToByte(sum % 256));

            var sbResult = new StringBuilder();
            foreach (byte b in hashBuffer)
            {
                sbResult.AppendFormat("{0:x2}", b);
            }
            return sbResult.ToString();
        }

        protected static byte[] GetLastBytes(byte[] array, int length)
        {
            if (array == null) return null;
            var newArray = new byte[length];
            for (int i = 0; i < length; i++)
            {
                newArray[i] = array[array.Length - length + i];
            }
            return newArray;
        }

        protected static byte[] Invert(byte[] array)
        {
            if (array == null) return null;
            var inverted = new byte[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                inverted[i] = array[array.Length - 1 - i];
            }
            return inverted;
        }

        private static int ComputeAddler32(byte[] buffer, int off, int len)
        {
            const int ADDLER32_BASE = 65521;

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            if (off < 0 || len < 0 || off + len > buffer.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            int checksum = 1;

            int s1 = checksum & 0xFFFF;
            int s2 = checksum >> 16;

            while (len > 0)
            {
                // We can defer the modulo operation:
                // s1 maximally grows from 65521 to 65521 + 255 * 3800
                // s2 maximally grows by 3800 * median(s1) = 2090079800 < 2^31
                int n = 3800;
                if (n > len)
                {
                    n = len;
                }
                len -= n;
                while (--n >= 0)
                {
                    s1 = s1 + (buffer[off++] & 0xFF);
                    s2 = s2 + s1;
                }
                s1 %= ADDLER32_BASE;
                s2 %= ADDLER32_BASE;
            }

            checksum = (s2 << 16) | s1;
            return checksum;
        }
    }
}