using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds.Levels
{
    public class BreakoutOverworld : OverWorld
    {
        protected override void CreateSubWorlds()
        {
            AddSubWorld(new BreakoutWorld());
        }
    }
}
