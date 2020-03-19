using PIGMServer.Game.Components;
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
            /*Collider collider = brick.Parent.Find<ColliderSystem>(SystemTypes.Collide);

            if(HitBall(collider))
            {
                brick.Health--;

                if (brick.Health == 0)
                    Destroy(brick);
            }*/
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
