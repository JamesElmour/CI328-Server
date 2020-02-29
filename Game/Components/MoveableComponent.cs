using PIGMServer.Game.Attributes;
using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class MoveableComponent : GameComponent
    {
        [Monitored]
        public Vector2 Velocity;
        
        public MoveableComponent(GameEntity parent, Vector2 velocity) : base(parent)
        {
            Velocity = velocity;
        }
    }
}
