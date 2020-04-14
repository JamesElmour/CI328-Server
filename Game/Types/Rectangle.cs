namespace PIGMServer.Game.Types
{
    public class Rectangle
    {
        private short x1, x2, y1, y2;

        public Rectangle(short x1, short x2, short y1, short y2)
        {
            this.x1 = x1;
            this.y1 = x2;
            this.x2 = y1;
            this.y2 = y2;
        }

        public Rectangle(Vector2 position, short width, short height)
        {
            short x = position.x;
            short y = position.y;

            x1 = x;
            x2 = (short) (x + width);
            y1 = y;
            y2 = (short) (y + height);
        }

        public static bool Intersects(Rectangle a, Rectangle b)
        {
            return !(a.x1 > b.x2) &&
                   !(a.x2 < b.x1) &&
                   !(a.y1 > b.y2) &&
                   !(a.y2 < b.y1);
        }
    }
}
