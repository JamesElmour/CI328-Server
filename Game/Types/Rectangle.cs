namespace PIGMServer.Game.Types
{
    /// <summary>
    /// Custom Rectangle datatype.
    /// </summary>
    public class Rectangle
    {
        // Hold Rectangle boundaries.
        private short x1, x2, y1, y2;

        /// <summary>
        /// Create a Rectangle with given bounds.
        /// </summary>
        /// <param name="x1">Left.</param>
        /// <param name="x2">Right.</param>
        /// <param name="y1">Top.</param>
        /// <param name="y2">Bottom.</param>
        public Rectangle(short x1, short x2, short y1, short y2)
        {
            this.x1 = x1;
            this.y1 = x2;
            this.x2 = y1;
            this.y2 = y2;
        }

        /// <summary>
        /// Create a Rectangle using the given properties.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public Rectangle(Vector2 position, short width, short height)
        {
            short x = position.x;
            short y = position.y;

            x1 = x;
            x2 = (short) (x + width);
            y1 = y;
            y2 = (short) (y + height);
        }

        /// <summary>
        /// Calculate if two Rectangles intersect.
        /// </summary>
        /// <param name="a">First Rectangle to check.</param>
        /// <param name="b">Second Rectangle to check.</param>
        /// <returns>If the Rectangles intersect.</returns>
        public static bool Intersects(Rectangle a, Rectangle b)
        {
            return !(a.x1 > b.x2) &&
                   !(a.x2 < b.x1) &&
                   !(a.y1 > b.y2) &&
                   !(a.y2 < b.y1);
        }
    }
}
