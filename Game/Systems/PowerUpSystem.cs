using PIGMServer.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Systems
{
    class PowerUpSystem : GameSystem<PowerUp>
    {
        public override SystemTypes GetSystemType()
        {
            return SystemTypes.PowerUp;
        }

        protected override void Process(PowerUp component, float deltaTime)
        {
            
        }
    }
}
