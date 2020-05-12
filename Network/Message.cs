using System;
using System.Linq;

namespace PIGMServer.Network
{
    #region OpCodes
    public enum SuperOps
    {
        Utility,
        Player,
        Ball,
        Brick,
        PowerUp,
    }

    public enum UtilityOps
    {
        SetPlayer,
        SetOpponent,
        Collated,
        Dead
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
        Create,
        Clear
    }

    public enum BrickOps
    {
        Hit,
        Destroyed,
        Spawn,
        Clear
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
    #endregion

    /// <summary>
    /// Message which contains data to be sent to a Client.
    /// </summary>
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

        public int Player = 1;  // If the Message is to be recieved by its player.
        public int SuperOp;     // Message's Super Op.
        public int SubOp;       // Message's Sub Op.
        private byte[] Data;    // Data bytes to be send to the Client.

        /// <summary>
        /// Create a Message with the provided player.
        /// </summary>
        /// <param name="superOp">Super opcode to send to the Client.</param>
        /// <param name="subOp">Message's sub opcode.</param>
        /// <param name="data">Data to send to a Client</param>
        public Message(int superOp, int subOp, byte[] data = null)
        {
            this.SuperOp = superOp;
            this.SubOp = subOp;
            Data = data;
        }

        /// <summary>
        /// Convert an array of data into a Message.
        /// </summary>
        /// <param name="data"></param>
        public Message(byte[] data)
        {
            if (data.Length > 2)
            {
                SuperOp = data[2];
                SubOp = data[3];
                Data = data.Skip(4).ToArray();
            }
        }

        /// <summary>
        /// Encode the Message into a byte array to be transmitted.
        /// </summary>
        /// <param name="requireRFC">If the Message requires a starting RFC Opcode</param>
        /// <returns>Encoded Message.</returns>
        public byte[] Encode(bool requireRFC = false)
        {
            // Reserved RFC opcode.
            byte rfcOp = 0b10000010;

            if (Data == null)
            {
                Data = Array.Empty<byte>();
            }

            byte length = (byte)(2 + Data.Length);
            byte[] encoded;

            // If the RFC is required then add it, otherwise only include super, sub, and data.
            if(requireRFC)
                encoded = new byte[] { rfcOp, length, (byte) (SuperOp + (128 * Player)), (byte)SubOp };
            else
                encoded = new byte[] { (byte)(SuperOp + (128 * Player)), (byte)SubOp };

            // Copy over encoded data into array.
            byte[] newData = new byte[encoded.Length + Data.Length];

            Array.Copy(encoded, newData, encoded.Length);
            Array.Copy(Data, 0, newData, encoded.Length, Data.Length);

            // Return data.
            return newData;
        }

        /// <summary>
        /// Get Message's super opcode.
        /// </summary>
        /// <returns>Super opcode.</returns>
        public int GetSuperOp()
        {
            return SuperOp;
        }

        /// <summary>
        /// Get Message's sub opcode.
        /// </summary>
        /// <returns>Sub opcode.</returns>
        public int GetSubOp()
        {
            return SubOp;
        }

        /// <summary>
        /// Get Message's data.
        /// </summary>
        /// <returns>Contained Data.</returns>
        public byte[] GetData()
        {
            return Data.ToArray();
        }
    }
}
