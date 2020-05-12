using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;
using PIGMServer.Utilities;
using System.Collections.Generic;

namespace PIGMServer.Game.Systems
{
    /// <summary>
    /// System which processes all Bricks for ECS technique.
    /// </summary>
    public class BrickSystem : GameSystem<Brick>
    {
        /// <summary>
        /// Create Brick system.
        /// </summary>
        /// <param name="world">Owning SubWorld.</param>
        public BrickSystem(SubWorld world) : base(world)
        {
            // Lower System priority to 2.
            Priority = 2;
        }

        /// <summary>
        /// Process the given Brick component.
        /// </summary>
        /// <param name="component">Ball component processing.</param>
        /// <param name="deltaTime">Deltatime.</param>
        protected override void Process(Brick brick, float deltaTime)
        {
            // Get Brick's collider.
            Collider collider = World.GetSystem<ColliderSystem>().Get(brick.Parent);

            if (HitBall(collider)) // If Collider has hit a Ball...
            {
                Hit(brick); // ... Process Ball hit.
            }
        }

        /// <summary>
        /// Process Hit for the given Brick.
        /// </summary>
        /// <param name="brick">Given Brick.</param>
        void Hit(Brick brick)
        {
            // Reduce Brick health.
            brick.Health--;

            // Set Brick has been altered.
            brick.Altered = true;

            if (brick.Health == 0) // If Brick has reached zero...
                brick.Parent.Destroy = true; // ... Destroy Brick.
        }

        /// <summary>
        /// Gather alterations for the given Brick.
        /// </summary>
        /// <param name="brick">Given Brick.</param>
        /// <returns>Message containing Brick information.</returns>
        protected override Message GatherAlterations(Brick brick)
        {
            // Set up Message Super and Sub ops.
            short superOp = (short)SuperOps.Brick;
            short subOp;
            
            // Add brick X and Y position to byte list.
            List<byte> data = new List<byte>();
            data.AddRange(Util.GetBytes(brick.X));
            data.AddRange(Util.GetBytes(brick.Y));

            if (brick.Health > 0) // If Brick not destroyed...
            {
                // ... Set Sub Op to Brick Hit and add Brick health to data.
                subOp = (short)BrickOps.Hit;
                data.AddRange(Util.GetBytes((short)brick.Health));
            }
            else // Otherwise...
            {
                // ... Set Brick destroyed.
                subOp = (short)BrickOps.Destroyed;
            }

            // Return generated Message.
            return new Message(superOp, subOp, data.ToArray());
        }

        /// <summary>
        /// Detect if given Collider has hit a Ball.
        /// </summary>
        /// <param name="collider">Given Collider.</param>
        /// <returns>If Collider hit Ball.</returns>
        private bool HitBall(Collider collider)
        {
            return (collider.Colliding && collider.CollidingWith.Contains("Ball"));
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Brick;
        }

        /// <summary>
        /// Get list of KeyValuePairs will all current alive Bricks.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<Vector2, int>> GetAliveBricks()
        {
            // Create housing list.
            List<KeyValuePair<Vector2, int>> bricks = new List<KeyValuePair<Vector2, int>>();

            foreach (Brick b in Components.Values) // For each Brick...
            {
                if (b.Health > 0) // ... And Brick is alive...
                {
                    // ... Get Brick position and health, then add to bricks list.
                    Vector2 position = new Vector2(b.X, b.Y);
                    int health = b.Health;

                    bricks.Add(new KeyValuePair<Vector2, int>(position, health));
                }
            }

            // Return alive Bricks.
            return bricks;
        }
    }
}
