using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{
    public class Player : GameComponent
    {
        public ushort Lives = 5;     // Number of lives Player has.

        /// <summary>
        /// Create Player.
        /// </summary>
        /// <param name="parent">Player's parent Entity.</param>
        public Player(GameEntity parent) : base(parent)
        {
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Player;
        }
    }
}
