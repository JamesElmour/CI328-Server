using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{
    /// <summary>
    /// Component which houses Brick data for ECS technique.
    /// </summary>
    public class Brick : GameComponent
    {

        private static readonly Vector2 Dimensions = new Vector2(64, 24); // Default size of brick.
        public readonly short X, Y;                                       // X and Y index position.
        public int Health = 3;                                            // Current brick health.

        /// <summary>
        /// Creates the Brick with the given X and Y position indexes.
        /// </summary>
        /// <param name="parent">Brick's parent.</param>
        /// <param name="x">X position in brick grid - not absolute position.</param>
        /// <param name="y">Y position in brick grid - not absolute position.</param>
        public Brick(GameEntity parent, int x, int y) : base(parent)
        {
            X = (short)x;
            Y = (short)y;
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Brick;
        }
    }
}
