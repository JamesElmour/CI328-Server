using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Network;
using PIGMServer.Utilities;

namespace PIGMServer.Game.Systems
{
    public class BallSystem : GameSystem<Ball>
    {
        public BallSystem(string worldName) : base(worldName)
        {
            
        }

        protected override void Process(Ball component, float deltaTime)
        {
            Collider collider = component.Parent.Get<Collider>(SystemTypes.Collider);

            PlayerBounce(component, collider);
            WallBounce(component);
            BrickBounce(component, collider);
            Move(component, deltaTime);
            component.Altered = true;
        }

        private void Move(Ball ball, float deltaTime)
        {
            Vector2 position = ball.Parent.Position;
            Vector2 velocity = new Vector2(0);
            float dirX = (ball.Direction.x - 100.0f) / 100.0f;
            float dirY = (ball.Direction.y - 100.0f) / 100.0f;
            velocity.x = (short) (dirX * ball.Speed * deltaTime);
            velocity.y = (short) (dirY * ball.Speed * deltaTime);

            position.x += velocity.x;
            position.y += velocity.y;

            ball.Parent.Position = position;
        }

        private void BrickBounce(Ball ball, Collider collider)
        {
            int foundIndex = collider.CollidingWith.IndexOf("Brick");

            if(foundIndex != -1)
            {
                Collider brick = (Collider) collider.CollidedComponents[foundIndex];
                Vector2 ballPos = ball.Parent.Position;
                Vector2 brickPos = brick.Parent.Position;
                KeyValuePair<float, float> collisionVector = new Vector2((short)(ballPos.x - (brickPos.x + 32)), (short) (ballPos.y - brickPos.y)).Normalize();
                float colVx = collisionVector.Key;
                float colVy = collisionVector.Value;

                if (Math.Abs(colVx) > 0.7)
                {
                    ball.Direction.x = (short) ((- (ball.Direction.x - 100)) + 100);

                    if(colVx < 0)
                    {
                        ballPos.x = brickPos.x;
                    }
                    else
                    {
                        ballPos.x = (short) (brickPos.x + 64);
                    }
                }
                else
                {
                    ball.Direction.y = (short) ((-(ball.Direction.y - 100)) + 100);

                    if (colVy < 0)
                    {
                        ballPos.y = brickPos.y;
                    }
                    else
                    {
                        ballPos.y = (short) (brickPos.y + 16);
                    }
                }

                ball.Altered = true;
                ball.Parent.Position = ballPos;
            }
        }

        private void WallBounce(Ball ball)
        {
            Vector2 position = ball.Parent.Position;

            if(position.x < 0 || position.x > 1280)
            {
                ball.Direction.x = (short)((-(ball.Direction.x - 100)) + 100);
                position.x = Math.Max(Math.Min(position.x, (short) 1280), (short) 0);
                ball.Altered = true;
            }

            if(position.y < 360 || position.y > 720)
            {
                ball.Direction.y = (short)((-(ball.Direction.y - 100)) + 100);
                position.y = Math.Max(Math.Min(position.y, (short) 720), (short) 360);
                ball.Altered = true;
            }
        }

        private void PlayerBounce(Ball ball, Collider collider)
        {
            int foundIndex = collider.CollidingWith.IndexOf("Player");
            if (foundIndex != -1)
            {
                Player player = (Player) collider.CollidedComponents[foundIndex];
                Vector2 position = collider.Parent.Position;
                Vector2 playerPos = player.Parent.Position;

                short newDirection = (short) (((position.x - playerPos.x) / 256) - 0.5f);
                ball.Direction.x = newDirection;
            }
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Ball;
        }

        protected override Message GatherAlterations(Ball ball)
        {
            short superOp = (short)SuperOps.Ball;
            short subOp = (short) BallOps.Bounce;

            short posX = ball.Parent.Position.x;
            short posY = ball.Parent.Position.y;
            byte[] posXData = Util.GetBytes(posX);
            byte[] posYData = Util.GetBytes(posY);
            byte[] dirXData = Util.GetBytes(ball.Direction.x);
            byte[] dirYData = Util.GetBytes(ball.Direction.y);

            return new Message(superOp, subOp, new byte[] { posXData[0], posXData[1], posYData[0], posYData[1], dirXData[0], dirXData[1], dirYData[0], dirYData[1]});
        }
    }
}
