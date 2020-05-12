using PIGMServer.Game.Systems;
using PIGMServer.Network;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game.Worlds
{
    public abstract class SubWorld : World
    {
        public Client Client = null;                                    // SubWorld's Client.
        public bool DummyWorld = false;                                 // If world is DummyWorld [For Testing]
        protected Dictionary<Type, IGameSystem> Systems;                // SubWorld's Systems.
        private List<Message> PreviousMessages = new List<Message>();   // List of Messages from previous GatherAlterations
        public SubWorld Opponent;                                       // SubWorld's opponent
        public int GatherPriority = 1;                                  // Current priority for gathering alteration messages.
        public int WorldIndex { get; private set; }                     // Index of the SubWorld in the SuperWorld.

        /// <summary>
        /// Create SubWorld with the given index and TCP Client.
        /// </summary>
        /// <param name="index">Index of SubWorld within SuperWorld</param>
        /// <param name="client">TCP Client for the SubWorld</param>
        public SubWorld(int index, Client client = null)
        {
            // Create Systems and assign attributes.
            Systems = new Dictionary<Type, IGameSystem>();
            Client = client;
            WorldIndex = index;
        }

        /// <summary>
        /// Create the SubWorld.
        /// </summary>
        public void Create()
        {
            // Setup SubWorld systems and components.
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
            foreach (IGameSystem system in Systems.Values)
            {
                system.Update(deltaTime);
            }
        }

        /// <summary>
        /// Add system to the subworld.
        /// </summary>
        /// <param name="system">The system added to the subworld.</param>
        public void AddSystem(IGameSystem system)
        {
            Systems.Add(system.GetType(), system);
        }

        /// <summary>
        /// Get the given System from the SubWorld.
        /// </summary>
        /// <typeparam name="T">Type of System to get.</typeparam>
        /// <returns>The requested System.</returns>
        public T GetSystem<T>() where T : IGameSystem
        {
            return (T) Systems[typeof(T)];
        }

        /// <summary>
        /// Gather alterations from SubWorld systems that are higher or equal priority to the provided priority.
        /// </summary>
        /// <param name="priority">System priority to gather.</param>
        /// <returns>List of messages from Systems.</returns>
        public List<Message> GatherAlterations(int priority)
        {
            // Setup List of messages.
            PreviousMessages = new List<Message>();

            foreach (IGameSystem system in Systems.Values) // For each System in SubWorld...
            {
                if (system.GetPriority() <= priority) // ... And they're under or equal to the priority...
                {
                    // Gather alterations from System.
                    PreviousMessages.AddRange(system.GetAlterations());
                }
            }

            // Return List of Messages.
            return PreviousMessages;
        }

        /// <summary>
        /// Gather alterations from previous gather.
        /// </summary>
        /// <returns>List of messages.</returns>
        public List<Message> GetPreviousAlterations()
        {
            return PreviousMessages;
        }
    }
}
