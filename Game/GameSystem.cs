using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Systems;
using PIGMServer.Network;

namespace PIGMServer.Game
{
    public abstract class GameSystem<T> : IGameSystem where T : IGameComponent
    {
        private const int ComponentLimit = 500;
        protected readonly Dictionary<string, T> Components = new Dictionary<string, T>(ComponentLimit);
        protected readonly List<T> AlteredComponents        = new List<T>(ComponentLimit);
        protected readonly List<T> TemporaryComponents      = new List<T>(ComponentLimit);
        protected readonly int     Priority                 = 1;
        protected readonly string  WorldName;

        public GameSystem(string worldName)
        {
            WorldName = worldName;
        }

        public void Update(float deltaTime)
        {
            PreprocessComponents();
            UpdateComponents(deltaTime);
        }
        
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


            foreach (T component in TemporaryComponents)
            {
                Process(component, deltaTime);
            }
        }

        public List<Message> GetAlterations()
        {
            List<Message> messageQueue = new List<Message>();
            AlteredComponents.Clear();

            foreach (T component in TemporaryComponents)
            {
                if (component.IsAltered())
                {
                    messageQueue.Add(GatherAlterations(component));
                }
            }

            return messageQueue;
        }

        protected abstract Message GatherAlterations(T alteredComponent);

        protected abstract void Process(T component, float deltaTime);

        public void Clear()
        {
            Components.Clear();
            TemporaryComponents.Clear();
            AlteredComponents.Clear();
        }

        public T Get(string name)
        {
            return Components[name];
        }

        public void Remove(string name)
        {
            Components.Remove(name);
        }

        public void Add(T component)
        {
            string name = component.GetName();
            Components.Add(name, component);

            SystemManager.MapComponentWorld(WorldName, component.GetName());
        }

        public void Add(IEnumerable<T> components)
        {
            foreach(T comp in components)
            {
                Add(comp);
            }
        }

        public int Count() => Components.Count;

        public abstract SystemTypes GetSystemType();
        public int GetPriority() => Priority;
    }
}
