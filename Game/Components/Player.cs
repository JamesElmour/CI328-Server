using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{
    public class Player : GameComponent
    {
        public short Direction = 1; // 0 = left, 1 = no movement, 2 = right
        public short Speed = 200;
        public new SystemTypes System = SystemTypes.Player;

        public Player(GameEntity parent) : base(parent)
        {
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Player;
        }
    }
}
