using System.Collections.Generic;

namespace PIGMServer.Game.Worlds
{
    class WorldSlice
    {
        private List<SubWorld> Worlds = new List<SubWorld>();

        public WorldSlice()
        { }

        public WorldSlice(List<SubWorld> worlds)
        {
            Worlds = worlds;
        }

        public void Update(float deltaTime)
        {
            foreach (SubWorld world in Worlds)
            {
                world.Update(deltaTime);
            }
        }

        public void Add(SubWorld world) => Worlds.Add(world);
        public void AddRange(IEnumerable<SubWorld> world) => Worlds.AddRange(world);
        public void Remove(SubWorld world) => Worlds.Remove(world);
        public SubWorld Get(int index) => Worlds[index];
    }
}
