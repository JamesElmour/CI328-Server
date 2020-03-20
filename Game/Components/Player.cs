using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{
    public class Player : GameComponent
    {
        public short Direction = 1; // 0 = left, 1 = no movement, 2 = right
        public short Speed = 200;

        public Player(GameEntity parent) : base(parent)
        {
            
        }
    }
}
