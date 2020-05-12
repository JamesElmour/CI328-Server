using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{

    /// <summary>
    /// Component which houses Ball data for ECS technique.
    /// </summary>
    public class Ball : GameComponent
    {
        public Vector2 Direction;                   // Direction the ball is moving in.
        public float Speed = 600;                   // Number of pixels per second speed.
        public bool WasCollidingWithPlayer = false; // Track if ball was touching player.
        public bool WasCollidingWithBrick = false;  // Track if ball was touching brick.
        public int Combo = 0;                       // Current combo, increases with brick hits.
        public int ID = 0;                          // ID of this ball.

        /// <summary>
        /// Create a Ball component under given parent.
        /// </summary>
        /// <param name="parent">Ball's parent.</param>
        public Ball(GameEntity parent) : base(parent)
        {
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Ball;
        }
    }
}
