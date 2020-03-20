using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Network
{
    public enum SuperOps
    {
        Player
    }

    public enum PlayerOps
    {
        DirectionChange,
        PositionUpdate
    }
    
    public class Message
    {
        public enum Opcode
        {
            Unknown,
            Connected,
            PlayerMove,
            PlayerUsePowerUp,
            PlayerReady
        }

        private readonly int SuperOp;
        private readonly int SubOp;
        private byte[] Data;

        public Message(int superOp, int subOp, byte[] data = null)
        {
            this.SuperOp = superOp;
            this.SubOp   = subOp;
            Data         = data;
        }

        public Message(byte[] data)
        {
            SuperOp = data[1];
            SubOp   = data[2];
            Data    = data.Skip(1).ToArray();
        }

        public byte[] Encode()
        {
            byte rfcOp = 0b10000001;

            if (Data == null)
            {
                Data = Array.Empty<byte>();
            }

            byte length = (byte)(2 + Data.Length);

            byte[] encoded = { rfcOp, length, (byte) SuperOp, (byte) SubOp };
            byte[] newData = new byte[4 + Data.Length];

            Array.Copy(encoded, newData, encoded.Length);
            Array.Copy(Data, 0, newData, 4, Data.Length);

            return newData;
        }

        public int GetSuperOp()
        {
            return SuperOp;
        }

        public int GetSubOp()
        {
            return SubOp;
        }

        public static Opcode GetOptcode(byte opByte)
        {

            switch (opByte)
            {
                case 1:
                    return Opcode.Connected;
                case 2:
                    return Opcode.PlayerMove;
                case 3:
                    return Opcode.PlayerUsePowerUp;
                case 4:
                    return Opcode.PlayerReady;

                default:
                    return Opcode.Unknown;
            }
        }

        public byte[] GetData()
        {
            return Data.Skip(3).ToArray();
        }
    }
}
