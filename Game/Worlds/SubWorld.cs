using PIGMServer.Game.Systems;
using PIGMServer.Network;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Worlds
{
    public abstract class SubWorld : World
    {
        public bool DummyWorld = false;
        protected Dictionary<Type, IGameSystem> Systems = new Dictionary<Type, IGameSystem>();
        protected Dictionary<string, GameEntity> Entities = new Dictionary<string, GameEntity>();
        private List<Message> PreviousMessages = new List<Message>();

        public int WorldIndex { get; private set; }

        public SubWorld(int index)
        {
            WorldIndex = index;
        }

        public MessageQueue Create()
        {
            MessageQueue queue = new MessageQueue();

            SetupSystems();
            SetupComponents();

            return queue;
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
            foreach (IGameSystem system in Systems.Values)
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
            Systems.Add(system.GetType(), system);
            //SystemManager.Add(Name, system);
        }

        public T GetSystem<T>() where T : IGameSystem
        {
            return (T) Systems[typeof(T)];
        }

        public List<Message> GatherAlterations(int priority)
        {
            PreviousMessages = new List<Message>();

            foreach (IGameSystem system in Systems.Values)
            {
                if (system.GetPriority() >= priority)
                {
                    PreviousMessages.AddRange(system.GetAlterations());
                }
            }

            return PreviousMessages;
        }

        public GameEntity GetEntity(string name)
        {
            return Entities[name];
        }

        public List<Message> GetPreviousAlterations()
        {
            return PreviousMessages;
        }
    }
}
