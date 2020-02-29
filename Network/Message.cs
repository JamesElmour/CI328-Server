using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Network
{
    public class Message
    {
        public enum Opcode
        {
            PlacePiece,
            Won,
            Lost,
            Connected,
            Waiting,
            Draw,
            Rematch,
            Unknown,
            GameReady
        }

        private readonly Opcode op;
        private byte[] Data;

        public Message(Opcode op, byte[] data = null)
        {
            this.op = op;
            Data = data;
        }

        public Message(byte[] data)
        {
            op = GetOptcode(data[0]);
            Data = data.Skip(1).ToArray();
        }

        public byte[] Encode()
        {
            byte rfcOp = 0b10000001;
            byte gameOp = 0;

            gameOp = (byte) op;

            if (Data == null)
            {
                Data = new byte[] { };
            }

            byte length = (byte)(1 + Data.Length);

            byte[] encoded = { rfcOp, length, gameOp };
            byte[] newData = new byte[3 + Data.Length];

            Array.Copy(encoded, newData, encoded.Length);
            Array.Copy(Data, 0, newData, 3, Data.Length);

            return newData;
        }


        public Opcode GetOptcode()
        {
            return op;
        }
        public Opcode GetOptcode(byte opByte)
        {

            switch (opByte)
            {
                case 1:
                    return Opcode.Connected;
                case 2:
                    return Opcode.Waiting;
                case 3:
                    return Opcode.PlacePiece;
                case 4:
                    return Opcode.Won;
                case 5:
                    return Opcode.Lost;
                case 6:
                    return Opcode.Draw;
                case 7:
                    return Opcode.Rematch;
                case 8:
                    return Opcode.Rematch;
                default:
                    return Opcode.Unknown;
            }
        }

        public byte[] GetData()
        {
            return Data.Skip(1).ToArray();
        }
    }
}
