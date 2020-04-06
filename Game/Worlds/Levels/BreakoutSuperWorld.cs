using PIGMServer.Game.Components;
using PIGMServer.Game.Systems;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds.Levels
{
    public class BreakoutSuperWorld : SuperWorld<BreakoutWorld>
    {
        protected override void CreateSubWorlds()
        {
            AddSubWorld(new BreakoutWorld(0));
            AcceptLimit = 1;
        }

        protected override void AddSubWorld(BreakoutWorld world)
        {

            if (SubWorlds.Count > 0)
            {
                SubWorlds[SubWorlds.Count - 1].ChangeOpponent(world);
            }

            base.AddSubWorld(world);

            if(IsFull && SubWorlds.Count > 1)
            {
                SubWorlds[SubWorlds.Count - 1].ChangeOpponent(SubWorlds[0]);
            }
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
    }
}
