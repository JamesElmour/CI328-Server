using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Components;
using PIGMServer.Network;

namespace PIGMServer.Game.Systems
{
    public class BallSystem : GameSystem<Ball>
    {
        public BallSystem(string worldName) : base(worldName)
        { }

        protected override void Process(Ball component, float deltaTime)
        {

        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Ball;
        }

        protected override Message GatherAlterations(Ball alteredComponent)
        {
            return new Message(1, 1);
        }
    }
}
