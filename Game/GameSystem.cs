using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Worlds;
using PIGMServer.Game.Worlds.Levels;
using PIGMServer.Network;

namespace PIGMServer.Game
{
    /// <summary>
    /// Process the housed Components with desired functionality.
    /// </summary>
    /// <typeparam name="T">Component to house.</typeparam>
    public abstract class GameSystem<T> : IGameSystem where T : IGameComponent
    {
        public readonly SubWorld World;     // The owning SubWorld.
        protected BreakoutWorld BreakWorld; // Owning BreakoutWorld.
        protected readonly Dictionary<string, T> Components = new Dictionary<string, T>();  // System's components.
        protected readonly List<T> AlteredComponents        = new List<T>();                // Altered components.
        protected readonly List<T> TemporaryComponents      = new List<T>();                // Temporary components during updating.

        public int Priority { get; protected set; } // Priority of the System.

        /// <summary>
        /// Create the System with the World as owning world.
        /// </summary>
        /// <param name="world">System's owner SubWorld.</param>
        public GameSystem(SubWorld world)
        {
            // Set up attributes.
            Priority = 1;
            World = world;
            BreakWorld = (BreakoutWorld) world;
        }

        /// <summary>
        /// Update the System's Components with DeltaTime.
        /// </summary>
        /// <param name="deltaTime">DeltaTime.</param>
        public void Update(float deltaTime)
        {
            PreprocessComponents();
            UpdateComponents(deltaTime);
        }
        
        /// <summary>
        /// Preprocess the System's Components prior to updating.
        /// </summary>
        private void PreprocessComponents()
        {
            foreach(T comp in Components.Values)
            {
                Preprocess(comp);
            }
        }

        protected virtual void Preprocess(T component)
        {}
        
       /// <summary>
       /// Cycle through available currently available components, calling their update methods.
       /// </summary>
       /// <param name="deltaTime">DeltaTime passed from parent GameOverWorld.</param>
        private void UpdateComponents(float deltaTime)
        {
            // Get current components, more may be generated during the updating cycle. Causing an error.
            TemporaryComponents.Clear();
            TemporaryComponents.AddRange(Components.Values);

            // Cycel through each Component.
            foreach (T component in TemporaryComponents)
            {
                if (!component.GameParent.Destroy) // If Component isn't destroyed...
                    Process(component, deltaTime); // ... Update component.
                else // ... Otherwise...
                    Components.Remove(component.GetName()); // ... Remove Component.
            }
        }

        /// <summary>
        /// Get alterations from System's Components and package into Messages.
        /// </summary>
        /// <returns></returns>
        public List<Message> GetAlterations()
        {
            // Create list of Messages.
            List<Message> messageQueue = new List<Message>();
            AlteredComponents.Clear();

            //For each Component...
            foreach (T component in TemporaryComponents)
            {   
                if(component.JustCreated()) // ... And is just created...
                {
                    // Add Message containing information on Component's creation to List.
                    messageQueue.Add(Create(component));
                }

                // Set Component to being old.
                component.IsOldNow();

                if (component.IsAltered()) // ... And has been altered.
                {
                    // Gather alterations and add to queue.
                    var message = GatherAlterations(component);

                    if(message != null)
                        messageQueue.Add(message);
                }
            }

            return messageQueue;
        }


        public virtual Message Create(T t)
        {
            return new Message(new byte[] { });
        }

        protected abstract Message GatherAlterations(T alteredComponent);

        protected abstract void Process(T component, float deltaTime);

        /// <summary>
        /// Get Component from System by Name.
        /// </summary>
        /// <param name="name">Component's name.</param>
        /// <returns>Found Component.</returns>
        public T Get(string name)
        {
            return Components[name];
        }

        /// <summary>
        /// Get Component in Parent.
        /// </summary>
        /// <param name="parent">Component's Parent Entity.</param>
        /// <returns>Found Component.</returns>
        public T Get(GameEntity parent)
        {
            return Get(parent.Name);
        }

        /// <summary>
        /// Add single component to the System.
        /// </summary>
        /// <param name="component">Component to add.</param>
        public void Add(T component)
        {
            string name = component.GetName();
            Components.Add(name, component);

            //component.GameParent.Add(component);
        }

        /// <summary>
        /// Add multiple components to System.
        /// </summary>
        /// <param name="components">Components.</param>
        public void Add(IEnumerable<T> components)
        {
            foreach(T comp in components)
            {
                Add(comp);
            }
        }

        /// <summary>
        /// Get the number of Components in the System.
        /// </summary>
        /// <returns>Number of Components in the System.</returns>
        public int Count() => Components.Count;

        public abstract SystemTypes GetSystemType();
        
        public int GetPriority() => Priority;
    }
}
