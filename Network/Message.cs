using System;
using System.Linq;

namespace PIGMServer.Network
{
    public enum SuperOps
    {
        Utility,
        Player,
        Ball,
        Brick,
        PowerUp
    }

    public enum UtilityOps
    {
        SetPlayer,
        SetOpponent,
    }

    public enum PlayerOps
    {
        DirectionChange,
        PositionUpdate,
        Spawn
    }

    public enum BallOps
    {
        Bounce,
        Create
    }

    public enum BrickOps
    {
        Hit,
        Destroyed,
        Spawn
    }

    public enum PowerUpOps
    {
        Activate,
        SpeedballCreated,
        SpeedballUsed,
        MultiBallCreated,
        MultiBallUsed,
        RapidBallCreated,
        RapidBallUsed,
        QuadBallCreated,
        QuadBallUsed,
        ExendPlayerCreated,
        ExendPlayerUsed,
        ShrinkPlayerCreated,
        ShrinkPlayerUsed,
        BeefyBricksCreated,
        BeefyBricksUsed,
        InvincibilityCreated,
        InvincibilityUsed
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

        public int Player = 1;
        private readonly int SuperOp;
        private readonly int SubOp;
        private byte[] Data;

        public Message(int superOp, int subOp, byte[] data = null)
        {
            this.SuperOp = superOp;
            this.SubOp = subOp;
            Data = data;
        }

        public Message(byte[] data)
        {
            if (data.Length > 2)
            {
                SuperOp = data[2];
                SubOp = data[3];
                Data = data.Skip(4).ToArray();
            }
        }

        public byte[] Encode()
        {
            byte rfcOp = 0b10000010;

            if (Data == null)
            {
                Data = Array.Empty<byte>();
            }

            byte length = (byte)(2 + Data.Length);

            byte[] encoded = { rfcOp, length, (byte) (SuperOp + (128 * Player)), (byte)SubOp };
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
            return Data.ToArray();
        }
    }
}
