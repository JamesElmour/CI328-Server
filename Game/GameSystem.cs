using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Systems;

namespace PIGMServer.Game
{
    public abstract class GameSystem<T> : IGameSystem where T : IGameComponent
    {
        private const int ComponentLimit = 500;
        protected readonly Dictionary<string, T> Components = new Dictionary<string, T>(ComponentLimit);
        protected readonly List<T> AlteredComponents        = new List<T>(ComponentLimit);
        protected readonly List<T> TemporaryComponents      = new List<T>(ComponentLimit);

        public GameSystem()
        {

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
            AlteredComponents.Clear();

            // Get current components, more may be generated during the updating cycle. Causing an error.
            TemporaryComponents.Clear();
            TemporaryComponents.AddRange(Components.Values);

            foreach (T component in TemporaryComponents)
            {
                Process(component, deltaTime);

                if (component.IsAltered())
                    AlteredComponents.Add(component);

            }
        }

        public void Clear()
        {
            Components.Clear();
            TemporaryComponents.Clear();
            AlteredComponents.Clear();
        }

        protected abstract void Process(T component, float deltaTime);

        public T Get(string name) => Components[name];
        public void Remove(string name) => Components.Remove(name);
        public abstract SystemTypes GetSystemType();
    }
}
