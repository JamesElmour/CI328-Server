using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Systems
{
    public static class SystemManager
    {
        [ThreadStatic]  // One instance per thread.
        private static readonly Dictionary<Type, IGameSystem> Systems =
            new Dictionary<Type, IGameSystem>();

        public static void Add(IGameSystem system)
        {
            Type systemType = system.GetType();
            if (Systems.ContainsKey(systemType))
            {
                Systems[systemType].Clear();
            }
            else
            {
                Systems.Add(system.GetType(), system);
            }
        }

        public static void Clear(Type system)
            => Systems[system].Clear();

        public static T GetComponent<T>(GameComponent component) where T : IGameComponent
        {
            GameSystem<IGameComponent> system =
                (GameSystem<IGameComponent>) Systems[component.System];
            return (T) system.GetComponent(component.Name);
        }
    }
}
