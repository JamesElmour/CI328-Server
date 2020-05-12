using PIGMServer.Game.Components;
using PIGMServer.Game.Worlds;
using PIGMServer.Game.Worlds.Levels;
using PIGMServer.Network;
using PIGMServer.Utilities;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Systems
{
    /// <summary>
    /// System which processes all Players for ECS technique.
    /// </summary>
    public class PlayerSystem : GameSystem<Player>
    {
        /// <summary>
        /// Create Player system.
        /// </summary>
        /// <param name="world">Owning SubWorld.</param>
        public PlayerSystem(SubWorld world) : base(world)
        { }

        /// <summary>
        /// Process the given Player component.
        /// </summary>
        /// <param name="component">Player component processing.</param>
        /// <param name="deltaTime">Deltatime.</param>
        protected override void Process(Player component, float deltaTime)
        {
            if (component.Lives <= 0) // If Player has no lives...
                ((BreakoutWorld) World).Dead = true; // ... Destroy player.
        }

        /// <summary>
        /// Gather alterations for the given Player.
        /// </summary>
        /// <param name="alteredComponent">Given Player.</param>
        /// <returns>Message containing Player information.</returns>
        protected override Message GatherAlterations(Player alteredComponent)
        {
            // Get Player's position and set Ops.
            short newPosition = alteredComponent.Parent.Position.x;
            short superOp = (short)SuperOps.Player;
            short subOp = (short)PlayerOps.PositionUpdate;
            
            // Add position to data bytes list.
            List<byte> bytes = new List<byte>();
            bytes.AddRange(Util.GetBytes(newPosition, 2));
            bytes.Add((byte) alteredComponent.Lives);

            // Create Messag with super op, sub op, and data.
            return new Message(superOp, subOp, bytes.ToArray());
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Player;
        }
    }
}
