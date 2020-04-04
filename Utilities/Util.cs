using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Utilities
{
    public class Util
    {
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
