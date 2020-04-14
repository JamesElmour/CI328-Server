using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game.Components
{
    public class Ball : GameComponent
    {
        public Vector2 Direction;
        public float Speed = 600;
        public bool WasCollidingWithPlayer = false;
        public bool WasCollidingWithBrick = false;

        public Ball(GameEntity parent) : base(parent)
        {
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Ball;
        }
    }
}
