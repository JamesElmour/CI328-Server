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
        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Player;
        }

        protected override void Process(Player component, float deltaTime)
        {
            
        }
    }
}
