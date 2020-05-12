using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;
using PIGMServer.Network;
using PIGMServer.UI;
using System;
using System.Collections.Generic;
using System.Threading;
using PIGMConsole;

namespace PIGMServer.Game.Worlds.Levels
{
    /// <summary>
    /// SuperWorld for the Breakout game mode.
    /// </summary>
    public class BreakoutSuperWorld : SuperWorld<BreakoutWorld>
    {
        /// <summary>
        /// Create the Breakout world.
        /// </summary>
        /// <param name="tps">Ticks per second of the SuperWorld.</param>
        public BreakoutSuperWorld(int tps = 30) : base(tps)
        {

        }

        /// <summary>
        /// Run pre-processing once the world is ready.
        /// </summary>
        protected override void PrepareWorld()
        {

        }

        /// <summary>
        /// Add the given Subworld and Client to the SuperWorld.
        /// </summary>
        /// <param name="world">Breakout SubWorld to add.</param>
        /// <param name="client">Breakout's TCP client.</param>
        protected override void AddSubWorld(BreakoutWorld world, Client client)
        {
            world.Create();
            CurrentWorld = world;
            if (SubWorlds.Count > 0)
            {
                SubWorlds[SubWorlds.Count - 1].ChangeOpponent(world);
            }

            base.AddSubWorld(world, client);

            if (IsFull && SubWorlds.Count > 1)
            {
                SubWorlds[SubWorlds.Count - 1].ChangeOpponent(SubWorlds[0]);
            }
        }

        /// <summary>
        /// Hand the given TCP Client to the SuperWorld.
        /// </summary>
        /// <param name="client"></param>
        public override void Give(Client client)
        {
            base.Give(client);

            AddSubWorld(new BreakoutWorld(Clients.IndexOf(client)), client);
        }

        /// <summary>
        /// Process the Player Position message.
        /// </summary>
        /// <param name="client">Client the message is from.</param>
        /// <param name="message">Message itself.</param>
        protected override void PlayerPositionChange(Client client, Message message)
        {
            // Get current world.
            SubWorld world = CurrentWorld;

            // Find the TCP Client's world.
            while (world.Client != client)
            {
                if(world.Opponent == null)
                {
                    world = null;
                    break;
                }
                world = world.Opponent;
            }

            // If Client World is found.
            if (world != null)
            {
                // Get the World's Player.
                GameEntity player = world.GetSystem<PlayerSystem>().Get("Player").Parent;

                // Calculate the new Player position.
                byte[] data = message.GetData();
                data = (data.Length == 2) ? data : new byte[] { 0, data[0] };
                data = new byte[] { data[1], data[0] };
                short pos = BitConverter.ToInt16(data, 0);

                // Update Player position.
                player.Position.x = pos;
            }
        }

        /// <summary>
        /// Process the activate PowerUp message.
        /// </summary>
        /// <param name="client">Client the message is from.</param>
        /// <param name="message">Message itself.</param>
        protected override void ActivatePowerUp(Client client, Message message)
        {
            // Get the Client's SubWorld and activate PowerUp.
            SubWorld world = SubWorlds[Clients.IndexOf(client)];
            world.GetSystem<PowerUpSystem>().ActivatePowerUp();
        }

        /// <summary>
        /// Transmit all alterations to current SubWorld for systems at given priority.
        /// </summary>
        /// <param name="priority">Required priority of systems for to have its messages gathered.</param>
        public override void TransmitAtlerations(int priority)
        {
            if (!CurrentWorld.DummyWorld) // If the current SubWorld isn't an inactive dummy world...
            {
                // Start Message Queue to hold produced Massages.
                MessageQueue queue = new MessageQueue();

                // Gather all alterations from current SubWorld and send them to Client.
                queue.Add(CurrentWorld.GatherAlterations(priority));
                SendQueue(CurrentWorld.Client, queue);

                // Create another Message Queue.
                queue = new MessageQueue();

                // Get previous transmitted alterations from opponent SubWorld.
                SubWorld opponent = CurrentWorld.Opponent;
                List<Message> messages = opponent.GetPreviousAlterations();

                // Set each message to be relating to the opponent.
                messages.ForEach((m) =>
                {
                    m.Player = 0;
                });

                if (messages.Count > 0) // If there are more than one messages...
                {
                    // ... Add them to the queue and send.
                    queue.Add(messages);
                    SendQueue(CurrentWorld.Client, queue);
                }
            }
            else // ... Otherwise...
            {
                // Gather current SubWorld alterations.
                CurrentWorld.GatherAlterations(priority);
            }
        }

        /// <summary>
        /// After the SubWorld has been updated.
        /// </summary>
        /// <param name="world">World to process after updating.</param>
        protected override void PostUpdate(BreakoutWorld world)
        {
            base.PostUpdate(world);

            if(((BreakoutWorld)world.Opponent).Dead) // If opponent World is dead...
            {
                // Create Message stating World is dead and send it to player.
                Message message = new Message((short) SuperOps.Utility, (short) UtilityOps.Dead);
                ClientAcceptor.SendMessage(world.Opponent.Client, message);

                // Remove this World.
                SubWorlds.Remove(world);

                if(world.Opponent.Opponent == world) // If there'll be one player left after this player is removed...
                {
                    // Set World opponent to won.
                    message = new Message(0, 2);
                    ClientAcceptor.SendMessage(world.Client, message);

                    // End the match and destroy this SuperWorld.
                    HasEnded = true;
                    Destroy = true;
                }
                else
                {
                    // Set World's opponent to the opponent's opponent.
                    world.Opponent = world.Opponent.Opponent;

                    // Clear all Balls and Bricks.
                    message = new Message((int) SuperOps.Ball, (int) BallOps.Clear);
                    ClientAcceptor.SendMessage(world.Client, message);

                    message = new Message((int) SuperOps.Brick, (int) BrickOps.Clear);
                    message.Player = 0;

                    ClientAcceptor.SendMessage(world.Client, message);

                    // Get all new opponent's alive bricks and spawn them.
                    var bricks = world.Opponent.GetSystem<BrickSystem>().GetAliveBricks();
                    foreach (KeyValuePair<Vector2, int> b in bricks)
                    {
                        byte x = (byte)b.Key.x;
                        byte y = (byte)b.Key.y;
                        byte h = (byte)b.Value;

                        message = new Message((int)SuperOps.Brick, (int)BrickOps.Spawn, new byte[] { x, y, h });
                        message.Player = 0;
                        ClientAcceptor.SendMessage(world.Client, message);
                    }

                    // Get all world's alive bricks and spawn them.
                    bricks = world.GetSystem<BrickSystem>().GetAliveBricks();
                    foreach (KeyValuePair<Vector2, int> b in bricks)
                    {
                        byte x = (byte)b.Key.x;
                        byte y = (byte)b.Key.y;
                        byte h = (byte)b.Value;

                        message = new Message((int)SuperOps.Brick, (int)BrickOps.Spawn, new byte[] { x, y, h });
                        ClientAcceptor.SendMessage(world.Client, message);
                    }
                }
            }
        }
    }
}
