using PIGMServer.Game.Attributes;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class Movable : GameComponent
    {
        [Monitored]
        public Vector2 Direction;
        public Vector2 Speed;
        
        public Movable(GameEntity parent, Vector2 direction) : base(parent)
        {
            Direction = direction;
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Ball;
        }
    }
}
