using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class Ball : GameComponent
    {
        public Vector2 Direction;
        public float Speed = 600;

        public Ball(GameEntity parent) : base(parent)
        {
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Ball;
        }
    }
}
