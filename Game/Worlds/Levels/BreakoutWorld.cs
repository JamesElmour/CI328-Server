using PIGMServer.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds.Levels
{
    public class BreakoutWorld : SubWorld
    {
        protected override void SetupSystems()
        {
            SystemManager.Add(new ColliderSystem());
        }
    }
}
