using PIGMServer.Game.Components;
using PIGMServer.Network;
using PIGMServer.Utilities;
using System.Collections.Generic;

namespace PIGMServer.Game.Systems
{
    public class BrickSystem : GameSystem<Brick>
    {
        public BrickSystem(string worldName) : base(worldName)
        { }

        protected override void Process(Brick brick, float deltaTime)
        {
            Collider collider = brick.Parent.Get<Collider>(SystemTypes.Collider);
            if (HitBall(collider))
            {
                Hit(brick);
            }
        }

        void Hit(Brick brick)
        {
            brick.Health--;
            brick.Altered = true;

            if (brick.Health == 0)
                brick.Parent.Destroy();
        }

        protected override Message GatherAlterations(Brick brick)
        {
            short superOp = (short)SuperOps.Brick;
            short subOp;
            List<byte> data = new List<byte>();
            data.AddRange(Util.GetBytes(brick.X));
            data.AddRange(Util.GetBytes(brick.Y));

            if (brick.Health > 0)
            {
                subOp = (short)BrickOps.Hit;
                data.AddRange(Util.GetBytes((short)brick.Health));
            }
            else
            {
                subOp = (short)BrickOps.Destroyed;
            }

            return new Message(superOp, subOp, data.ToArray());
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
