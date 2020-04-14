﻿using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Types
{
    public class Vector2
    {
        public static readonly Vector2 Zero = new Vector2(0);
        public short x, y;

        public Vector2(short a)
        {
            x = y = a;
        }

        public Vector2(short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        public KeyValuePair<float, float> Normalize()
        {
            float magnitude = (Math.Abs(this.x) + Math.Abs(this.y));
            float x = this.x / magnitude;
            float y = this.y / magnitude;

            return new KeyValuePair<float, float>(x, y);
        }

        public byte[] ToByteArray()
        {
            byte[] VectorBytes = new byte[4];
            byte[] bX = BitConverter.GetBytes(x);
            byte[] bY = BitConverter.GetBytes(y);

            VectorBytes[0] = bX[0];
            if (bX.Length == 1)
            {
                VectorBytes[1] = 0;
            }
            else
            {
                VectorBytes[1] = bX[1];
            }

            VectorBytes[2] = bY[0];
            if (bY.Length == 1)
            {
                VectorBytes[3] = 0;
            }
            else
            {
                VectorBytes[3] = bY[1];
            }

            return VectorBytes;
        }

        #region Operators
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x + b.x), (short)(a.x + b.x));
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x - b.x), (short)(a.x - b.x));
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x * b.x), (short)(a.x * b.x));
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x / b.x), (short)(a.x / b.x));
        }
        public static Vector2 operator +(Vector2 a, float b)
        {
            return new Vector2((short)(a.x + b), (short)(a.x + b));
        }

        public static Vector2 operator -(Vector2 a, float b)
        {
            return new Vector2((short)(a.x - b), (short)(a.x - b));
        }

        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2((short)(a.x * b), (short)(a.x * b));
        }

        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2((short)(a.x / b), (short)(a.x / b));
        }
        #endregion
    }
}
