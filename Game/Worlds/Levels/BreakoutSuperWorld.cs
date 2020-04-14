using PIGMServer.Network;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Worlds.Levels
{
    public class BreakoutSuperWorld : SuperWorld<BreakoutWorld>
    {
        public BreakoutSuperWorld(int tps = 60) : base(tps)
        {

        }

        protected override void CreateSubWorlds()
        {
            AddSubWorld(new BreakoutWorld(0));
            AcceptLimit = 1;
        }

        protected override void AddSubWorld(BreakoutWorld world)
        {
            world.Create();
            if (SubWorlds.Count > 0)
            {
                SubWorlds[SubWorlds.Count - 1].ChangeOpponent(world);
            }

            base.AddSubWorld(world);

            if (IsFull && SubWorlds.Count > 1)
            {
                SubWorlds[SubWorlds.Count - 1].ChangeOpponent(SubWorlds[0]);
            }
        }

        public override void Give(Client client)
        {
            base.Give(client);

            BreakoutWorld world = new BreakoutWorld(1);
            world.DummyWorld = true;
            world.Create();
            SubWorlds[SubWorlds.Count - 1].ChangeOpponent(world);
            SubWorlds.Add(world);
        }

        protected override void PlayerPositionChange(Client client, Message message)
        {
            SubWorld world = SubWorlds[Clients.IndexOf(client)];
            GameEntity player = world.GetEntity("Player");

            byte[] data = message.GetData();
            data = (data.Length == 2) ? data : new byte[] { 0, data[0] };
            data = new byte[] { data[1], data[0] };
            short pos = BitConverter.ToInt16(data, 0);

            player.Position.x = pos;
        }

        public override void TransmitAtlerations(int priority)
        {
            BreakoutWorld world = SubWorlds[SubworldIndex];

            if (!world.DummyWorld)
            {
                MessageQueue queue = new MessageQueue();

                queue.Add(world.GatherAlterations(priority));
                SendQueue(Get(SubworldIndex), queue);

                queue = new MessageQueue();

                BreakoutWorld opponent = world.Opponent;
                List<Message> messages = opponent.GetPreviousAlterations();
                messages.ForEach((m) =>
                {
                    m.Player = 0;
                });

                if (messages.Count > 0)
                {
                    queue.Add(messages);
                    SendQueue(Get(SubworldIndex), queue);
                }
            }
            else
            {
                world.GatherAlterations(priority);
            }
        }
    }
}
