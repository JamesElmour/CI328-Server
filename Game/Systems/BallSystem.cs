using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;
using PIGMServer.Utilities;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Systems
{
    /// <summary>
    /// System which processes all Balls for ECS technique.
    /// </summary>
    public class BallSystem : GameSystem<Ball>
    {
        /// <summary>
        /// Create Ball system.
        /// </summary>
        /// <param name="world">Owning SubWorld.</param>
        public BallSystem(SubWorld world) : base(world)
        {
        }

        /// <summary>
        /// Process the given Ball component.
        /// </summary>
        /// <param name="component">Ball component processing.</param>
        /// <param name="deltaTime">Deltatime.</param>
        protected override void Process(Ball component, float deltaTime)
        {
            // Get Ball's Collider.
            Collider collider = World.GetSystem<ColliderSystem>().Get(component.Parent);

            PlayerBounce(component, collider);
            WallBounce(component);
            BrickBounce(component, collider);
            Move(component, deltaTime);

            // Ball has moved thus has been altered.
            component.Altered = true;
        }

        /// <summary>
        /// Move the Ball given its direction, speed and DeltaTime.
        /// </summary>
        /// <param name="ball">Given Ball.</param>
        /// <param name="deltaTime">DeltaTime.</param>
        private void Move(Ball ball, float deltaTime)
        {
            // Get Ball's potioion and velocity.
            Vector2 position = ball.Parent.Position;
            Vector2 velocity = new Vector2(0);

            // Normalize Ball's velocity from 0-200 to -1.0-1.0...
            float dirX = (ball.Direction.x - 100.0f) / 100.0f;
            float dirY = (ball.Direction.y - 100.0f) / 100.0f;

            // .. Then apply Speed and DeltaTime.
            velocity.x = (short)(dirX * ball.Speed * deltaTime);
            velocity.y = (short)(dirY * ball.Speed * deltaTime);

            // Update position by velocity.
            position.x += velocity.x;
            position.y += velocity.y;

            // Apply translation.
            ball.Parent.Position = position;
        }

        /// <summary>
        /// Bounce the Ball if it has collided with a Brick.
        /// </summary>
        /// <param name="ball">Given Ball.</param>
        /// <param name="collider">Ball's Collider.</param>
        private void BrickBounce(Ball ball, Collider collider)
        {
            // Find index of 'Brick' tag in Collider's collided with tags.
            int foundIndex = collider.CollidingWith.IndexOf("Brick");

            // If ball was not colliding with a Brick previously and is now colliding with a Brick...
            if (!ball.WasCollidingWithBrick && foundIndex != -1)
            {
                // Get Brick's Collider.
                Collider brick = (Collider)collider.CollidedComponents[foundIndex];

                // Get Ball and Brick's position.
                Vector2 ballPos = ball.Parent.Position;
                Vector2 brickPos = brick.Parent.Position;

                // Calculate Collision Vector between Ball and Brick.
                KeyValuePair<float, float> collisionVector = new Vector2((short)(ballPos.x - (brickPos.x + 32)), (short)(ballPos.y - brickPos.y)).Normalize();
                float colVx = collisionVector.Key;

                // If collision has occured on the Brick's side...
                if (Math.Abs(colVx) > 0.7)
                {
                    // ... Invert Ball's X movement direction.
                    ball.Direction.x = (short)((-(ball.Direction.x - 100)) + 100);
                }
                else // ... Otherwise
                {
                    // ... Invert Ball's Y movement direction.
                    ball.Direction.y = (short)((-(ball.Direction.y - 100)) + 100);
                }

                // Set Ball colliding with Brick.
                ball.WasCollidingWithBrick = true;

                // Increase Ball's cobo.
                ball.Combo++;
            }
            else // Otherwise...
            {

                // Set Ball not colliding with Brick.
                ball.WasCollidingWithBrick = false;
            }
        }

        /// <summary>
        /// Process given Ball's wall bounce.
        /// </summary>
        /// <param name="ball">Given Ball.</param>
        private void WallBounce(Ball ball)
        {
            // Get Ball's position.
            Vector2 position = ball.Parent.Position;

            if (position.x < 0 || position.x > 1280) // If Ball is outside left/right screen...
            {                
                // Invert the Ball's X direction and cap within boundaries.
                ball.Direction.x = (short)((-(ball.Direction.x - 100)) + 100);
                position.x = Math.Max(Math.Min(position.x, (short) 1280), (short )0);
            }

            if (position.y < 360 || position.y > 720) // If Ball is outside tpo/bottom screen...
            {
                if (position.y > 720) // ... And Ball has dropped below player.
                    World.GetSystem<PlayerSystem>().Get("Player").Lives--; // Deduct a life from the Player.

                // Invert the Ball's Y direction and cap within boundaries.
                ball.Direction.y = (short)((-(ball.Direction.y - 100)) + 100);
                position.y = Math.Max(Math.Min(position.y, (short) 720), (short) 360);
            }
        }

        /// <summary>
        /// Process Ball bounced against Player.
        /// </summary>
        /// <param name="ball">Given Ball.</param>
        /// <param name="collider">Ball's Collider.</param>
        private void PlayerBounce(Ball ball, Collider collider)
        {
            // Find index of 'Player' tag in Collider's collided with tags.
            int foundIndex = collider.CollidingWith.IndexOf("Player");

            // If ball was not colliding with a Player previously and is now colliding with a Player...
            if (foundIndex != -1 && !ball.WasCollidingWithPlayer)
            {
                // Get Collided with Player component.
                Collider player = (Collider)collider.CollidedComponents[foundIndex];
                
                // Get Ball and Player position.
                Vector2 position = collider.Parent.Position;
                Vector2 playerPos = player.Parent.Position;

                // Calculate Ball's position offset according to Player's position - for aiming.
                float newDirection = ((((float)position.x - (float)playerPos.x) / 256) - 0.5f);
                newDirection = ((100 * newDirection) + 100);

                // Apply new aimed X direction and invert ball Y direction.
                ball.Direction.x = (short)newDirection;
                ball.Direction.y = (short)((-(ball.Direction.y - 100)) + 100);

                // Snap Ball position to above Player.
                ball.Parent.Position.y = (short)(player.Parent.Position.y - 24);

                // Set was colliding with Player.
                ball.WasCollidingWithPlayer = true;

                // Create combo with given Ball combo.
                World.GetSystem<PowerUpSystem>().CreatePowerup(ball.Combo);
                
                // Reset combo.
                ball.Combo = 0;
            }
            else if (ball.WasCollidingWithPlayer) // OTherwise...
            {
                // Set ball wasn't colliding with Player.
                ball.WasCollidingWithPlayer = false;
            }
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Ball;
        }

        protected override Message GatherAlterations(Ball ball)
        {
            return Bounced(ball);
        }

        /// <summary>
        /// Create a message which creates Ball component.
        /// </summary>
        /// <param name="ball">Given Ball.</param>
        /// <returns>Message with created Ball information.</returns>
        public override Message Create(Ball ball)
        {
            // Get Ball data from Bounced method.
            Message message = Bounced(ball);

            // Change Message SubOp.
            message.SubOp = (short) BallOps.Create;


            // Return message creating Ball.
            return message;
        }

        /// <summary>
        /// Create a message when Ball has bounced.
        /// </summary>
        /// <param name="ball">Given Ball.</param>
        /// <returns>Message containing Ball information.</returns>
        private Message Bounced(Ball ball)
        {
            // Set Super/Sub ops.
            byte superOp = (byte)SuperOps.Ball;
            byte subOp = (byte)BallOps.Bounce;

            // Get Ball; position, ID, and direction in byte form.
            short posX = ball.Parent.Position.x;
            short posY = ball.Parent.Position.y;
            byte id = (byte)ball.ID;
            byte[] posXData = Util.GetBytes(posX);
            byte[] posYData = Util.GetBytes(posY);
            byte[] dirXData = Util.GetBytes(ball.Direction.x);
            byte[] dirYData = Util.GetBytes(ball.Direction.y);

            // Create new Message with super opcode, sub opcode, and data bytes. 
            return new Message(superOp, subOp, new byte[] { posXData[0], posXData[1], posYData[0], posYData[1], id, dirXData[0], dirXData[1], dirYData[0], dirYData[1] });
        }

        /// <summary>
        /// Set all Ball speed to given speed.
        /// </summary>
        /// <param name="speed">Speed to set Ball speed as.</param>
        public void SetSpeed(int speed)
        {
            // Cycle through Balls and set speed.
            foreach (Ball ball in Components.Values)
            {
                ball.Speed = speed;
            }
        }
    }
}
