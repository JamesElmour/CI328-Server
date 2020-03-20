using PIGMServer.Game.Components;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Systems
{
    public class PlayerSystem : GameSystem<Player>
    {
        public PlayerSystem(string worldName) : base(worldName)
        { }

        protected override void Process(Player component, float deltaTime)
        {
            GameEntity parent = component.Parent;
            short speed = component.Speed;
            short direction = (short) (component.Direction - 1);
            short velocity  = (short) (direction * speed * deltaTime);

            parent.Position.x += velocity;
            component.Altered = true;
        }

        protected override Message GatherAlterations(Player alteredComponent)
        {
            short newPosition = alteredComponent.Parent.Position.x;
            short superOp = (short) SuperOps.Player;
            short subOp   = (short) PlayerOps.PositionUpdate;

            return new Message(superOp, subOp, new byte[] { (byte) newPosition });
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Player;
        }
    }
}
