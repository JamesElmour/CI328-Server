using System;

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
