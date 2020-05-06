using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{
    public class Player : GameComponent
    {
        public ushort Direction = 1; // 0 = left, 1 = no movement, 2 = right
        public ushort Speed = 200;
        public ushort Lives = 3;

        public Player(GameEntity parent) : base(parent)
        {
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Player;
        }
    }
}
