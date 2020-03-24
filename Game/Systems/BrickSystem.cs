using PIGMServer.Game.Components;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Systems
{
    public class BrickSystem : GameSystem<Brick>
    {
        public BrickSystem(string worldName) : base(worldName)
        { }

        protected override void Process(Brick brick, float deltaTime)
        {
            Collider collider = brick.Parent.Get<Collider>(SystemTypes.Collide);
            if(HitBall(collider))
            {
                Hit(brick);
            }
        }

        void Hit(Brick brick)
        {
            brick.Health--;
            brick.Parent.Destroy();
        }

        protected override Message GatherAlterations(Brick alteredComponent)
        {
            return new Message(1, 1);
        }

        private bool HitBall(Collider collider)
        {
            return (collider.Colliding && collider.CollidingWith.Contains("Ball"));
        }

        private void Destroy(Brick brick)
        {
            brick.Parent.Destroy();
        }
        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Brick;
        }
    }
}
