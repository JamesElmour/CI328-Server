using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Network;

namespace PIGMServer.Game.Systems
{
    public class BallSystem : GameSystem<Ball>
    {
        public BallSystem(string worldName) : base(worldName)
        {
            
        }

        protected override void Process(Ball component, float deltaTime)
        {
            Collider collider = component.Parent.Get<Collider>(SystemTypes.Collide);

            PlayerBounce(component, collider);
            WallBounce(component);
            BrickBounce(component, collider);
            Move(component, deltaTime);
        }

        private void Move(Ball ball, float deltaTime)
        {
            Vector2 position = ball.Parent.Position;
            Vector2 velocity = new Vector2(0);
            velocity.x = (short) (ball.Direction.x * ball.Speed * deltaTime);
            velocity.y = (short) (ball.Direction.y * ball.Speed * deltaTime);

            position.x += velocity.x;
            position.y += velocity.y;

            ball.Parent.Position = position;
        }

        private void BrickBounce(Ball ball, Collider collider)
        {
            int foundIndex = collider.CollidingWith.IndexOf("Brick");

            if(foundIndex != -1)
            {
                Brick brick = (Brick) collider.CollidedComponents[foundIndex];
                Vector2 ballPos = ball.Parent.Position;
                Vector2 brickPos = brick.Parent.Position;
                Vector2 collisionVector = new Vector2((short)(ballPos.x - (brickPos.x + 32)), (short) (ballPos.y - brickPos.y)).Normalize();

                if(Math.Abs(collisionVector.x) > 0.7)
                {
                    ball.Direction.x = (short) -ball.Direction.x;

                    if(collisionVector.x < 0)
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
                    ball.Direction.y = (short) -ball.Direction.y;

                    if (collisionVector.y < 0)
                    {
                        ballPos.y = brickPos.y;
                    }
                    else
                    {
                        ballPos.y = (short) (brickPos.y + 16);
                    }
                }

                ball.Parent.Position = ballPos;
            }
        }

        private void WallBounce(Ball ball)
        {
            Vector2 position = ball.Parent.Position;

            if(position.x < 0 || position.x > 1280)
            {
                ball.Direction.x = (short) -ball.Direction.x;
                position.x = Math.Max(Math.Min(position.x, (short) 1280), (short) 0);
            }

            if(position.y < 360 || position.y > 720)
            {
                ball.Direction.y = (short) -ball.Direction.y;
                position.y = Math.Max(Math.Min(position.y, (short) 720), (short) 360);
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

        protected override Message GatherAlterations(Ball alteredComponent)
        {
            return new Message(1, 1);
        }
    }
}
