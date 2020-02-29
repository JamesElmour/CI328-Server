using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds
{
    public abstract class SubWorld
    {
        private Dictionary<string, GameSystem<GameComponent>> systems = new Dictionary<string, GameSystem<GameComponent>>();

        public SubWorld()
        {
            SetupSystems();
        }

        /// <summary>
        /// Set up the SubWorld's systems, relient on sub-classes.
        /// </summary>
        protected abstract void SetupSystems();

        /// <summary>
        /// Update the SubWorld.
        /// </summary>
        /// <param name="deltaTime">Deltatime passed by the OverWorld.</param>
        public void Update(float deltaTime)
        {
            UpdateSystems(deltaTime);
        }

        /// <summary>
        /// Update the SubWorld's systems, in the defined order with the given deltatime.
        /// </summary>
        /// <param name="deltaTime">Deltatime passed by the OverWorld.</param>
        private void UpdateSystems(float deltaTime)
        {
            foreach(GameSystem<GameComponent> system in systems.Values)
            {
                system.Update(deltaTime);
            }
        }
    }
}
