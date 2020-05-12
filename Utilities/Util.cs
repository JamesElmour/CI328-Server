using System;
using System.Collections.Generic;

namespace PIGMServer.Utilities
{
    /// <summary>
    /// Utility class for various useful functions.
    /// </summary>
    public class Util
    {
        /// <summary>
        /// Convert the given short into a number of bytes.
        /// </summary>
        /// <param name="value">Short to convert.</param>
        /// <param name="numberOfBytes">Number of bytes to return.</param>
        /// <returns></returns>
        public static byte[] GetBytes(short value, int numberOfBytes = 2)
        {
            List<byte> data = new List<byte>(BitConverter.GetBytes(value));
            int count = data.Count;

            while(count < numberOfBytes)
            {
                data.Insert(0, 0);
                count = data.Count;
            }

            return data.ToArray();
        }
    }
}
