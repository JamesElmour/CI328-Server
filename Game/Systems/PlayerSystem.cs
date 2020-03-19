using PIGMServer.Game.Components;
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
            short velocity  = (short) (direction * speed);

            parent.Position.x += velocity;
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Player;
        }
    }
}
