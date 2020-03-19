using PIGMServer.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds
{
    public abstract class SubWorld : World
    {
        private Dictionary<string, IGameSystem> systems = new Dictionary<string, IGameSystem>();

        public SubWorld()
        {
            SetupSystems();
            SetupComponents();
        }

        /// <summary>
        /// Set up the SubWorld's systems, relient on sub-classes.
        /// </summary>
        protected abstract void SetupSystems();

        /// <summary>
        /// Set up components for the SubWorld.
        /// </summary>
        protected abstract void SetupComponents();

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
            foreach(IGameSystem system in systems.Values)
            {
                system.Update(deltaTime);
            }
        }

        /// <summary>
        /// Add system to the subworld.
        /// </summary>
        /// <param name="system">The system added to the subworld.</param>
        protected void AddSystem(IGameSystem system)
        {
            string name = system.GetSystemType().ToString();
            systems.Add(name, system);

            SystemManager.Add(Name, system);
        }
    }
}
