using PIGMServer.Game.Components;
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
            AddSystem(new PlayerSystem(Name));
            AddSystem(new MoveableSystem(Name));
            AddSystem(new ColliderSystem(Name));
            AddSystem(new BallSystem(Name));
        }

        /// <summary>
        /// Create components for the world.
        /// </summary>
        protected override void SetupComponents()
        {
            CreatePlayer();
        }

        /// <summary>
        /// Set up the main Player entity.
        /// </summary>
        private void CreatePlayer()
        {
            PlayerSystem playerSystem   = SystemManager.GetSystem<PlayerSystem>(Name);
            GameEntity   entity         = new GameEntity("Player");
            Player       player         = new Player(entity);

            playerSystem.Add(player);
        }
    }
}
