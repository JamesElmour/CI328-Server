using PIGMServer.Game.Systems;
using PIGMServer.Network;
using System.Collections.Generic;

namespace PIGMServer.Game.Worlds
{
    public abstract class SubWorld : World
    {
        protected Dictionary<string, IGameSystem> Systems = new Dictionary<string, IGameSystem>();
        protected Dictionary<string, GameEntity> Entities = new Dictionary<string, GameEntity>();
        private MessageQueue Queue = new MessageQueue();
        public int WorldIndex { get; private set; }

        public SubWorld(int index)
        {
            WorldIndex = index;
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
            foreach(IGameSystem system in Systems.Values)
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
            Systems.Add(name, system);

            SystemManager.Add(Name, system);
        }

        public T GetSystem<T>() where T : IGameSystem
        {
            string name = typeof(T).Name;
            return (T) Systems[name];
        }

        public List<Message> GatherAlterations(int priority)
        {
            List<Message> messages = new List<Message>();
            
            foreach (IGameSystem system in Systems.Values)
            {
                if(system.GetPriority() >= priority)
                {
                    messages.AddRange(system.GetAlterations());
                }
            }

            return messages;
        }

        public GameEntity GetEntity(string name)
        {
            return Entities[name];
        }
    }
}
