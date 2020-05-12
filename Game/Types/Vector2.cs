using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Types
{
    /// <summary>
    /// Custom 2 dimensional vector datatype.
    /// </summary>
    public class Vector2
    {
        // Static Vector2 with 0 X and Y values.
        public static readonly Vector2 Zero = new Vector2(0);
        public short x, y; // X and Y values.

        /// <summary>
        /// Create a Vector2 with same X and Y values.
        /// </summary>
        /// <param name="a">Value to set X and Y values as.</param>
        public Vector2(short a)
        {
            x = y = a;
        }

        /// <summary>
        /// Create a Vector2 with given X and Y values.
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public Vector2(short x, short y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Normalize this Vector2 to between 0 and 1.
        /// </summary>
        /// <returns>Normalized values.</returns>
        public KeyValuePair<float, float> Normalize()
        {
            float magnitude = (Math.Abs(this.x) + Math.Abs(this.y));
            float x = this.x / magnitude;
            float y = this.y / magnitude;

            return new KeyValuePair<float, float>(x, y);
        }


        #region Operators

        /// <summary>
        /// Add two Vector2s.
        /// </summary>
        /// <param name="a">First Vector2 to add.</param>
        /// <param name="b">Second Vector2 to add.</param>
        /// <returns>New Vector2 with added values.</returns>
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x + b.x), (short)(a.x + b.x));
        }

        /// <summary>
        /// Subtract two Vector2s.
        /// </summary>
        /// <param name="a">First Vector2 to subtract.</param>
        /// <param name="b">Second Vector2 to subtract.</param>
        /// <returns>New Vector2 with subtracted values.</returns>
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x - b.x), (short)(a.x - b.x));
        }

        /// <summary>
        /// Multiply two Vector2s.
        /// </summary>
        /// <param name="a">First Vector2 to multiply.</param>
        /// <param name="b">Second Vector2 to multiply.</param>
        /// <returns>New Vector2 with multiplied values.</returns>
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x * b.x), (short)(a.x * b.x));
        }
        /// <summary>
        /// Divide two Vector2s.
        /// </summary>
        /// <param name="a">First Vector2 to divide.</param>
        /// <param name="b">Second Vector2 to divide.</param>
        /// <returns>New Vector2 with divided values.</returns>
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2((short)(a.x / b.x), (short)(a.x / b.x));
        }

        /// <summary>
        /// Add Vector2 with float.
        /// </summary>
        /// <param name="a">Base Vector2.</param>
        /// <param name="b">Float to add to Vector2.</param>
        /// <returns>New Vector2 with added float.</returns>
        public static Vector2 operator +(Vector2 a, float b)
        {
            return new Vector2((short)(a.x + b), (short)(a.x + b));
        }

        /// <summary>
        /// Subtract float from Vector2.
        /// </summary>
        /// <param name="a">Base Vector2.</param>
        /// <param name="b">Float to subtract from Vector2.</param>
        /// <returns>New Vector2 with subtracted float.</returns>
        public static Vector2 operator -(Vector2 a, float b)
        {
            return new Vector2((short)(a.x - b), (short)(a.x - b));
        }

        /// <summary>
        /// Multiply Vector2 with float.
        /// </summary>
        /// <param name="a">Base Vector2.</param>
        /// <param name="b">Float to multiply to Vector2.</param>
        /// <returns>New Vector2 with multiplied float.</returns>
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2((short)(a.x * b), (short)(a.x * b));
        }

        /// <summary>
        /// Divide Vector2 by float.
        /// </summary>
        /// <param name="a">Base Vector2.</param>
        /// <param name="b">Float to divide Vector2 by.</param>
        /// <returns>New Vector2 divided by float.</returns>
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2((short)(a.x / b), (short)(a.x / b));
        }
        #endregion
    }
}
