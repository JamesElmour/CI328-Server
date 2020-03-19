using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds
{
    public abstract class World
    {
        public string Name { get; private set; }

        public World()
        {
            Guid id = Guid.NewGuid();
            Name = id.ToString();
        }
    }
}
