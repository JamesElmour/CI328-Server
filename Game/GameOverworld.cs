using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    public class GameOverWorld
    {
        private List<GameSubWorld> SubWorlds = new List<GameSubWorld>();
        private readonly int Tps;
        private readonly int SubworldsPerFrame;
        private readonly float Deltatime;

        public GameOverWorld(int tps = 20)
        {
            Tps = tps;
            Deltatime = 1 / tps;
        }

        public void Start()
        {

        }

        public void Update()
        {
            UpdateSubWorlds();
        }
        
        private void UpdateSubWorlds()
        {
            foreach(GameSubWorld world in SubWorlds)
            {
                world.Update(Deltatime);
            }
        }
    }
}
