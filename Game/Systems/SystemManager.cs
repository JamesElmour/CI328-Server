using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PIGMServer.Game.Systems
{
    public enum SystemTypes
    {
        Ball,
        Brick,
        Collider,
        Movable,
        Player,
        PowerUp,
        Unknown
    }

    public static class SystemManager
    {
        //[ThreadStatic]  // One instance per thread.
        private static readonly Dictionary<string, Dictionary<SystemTypes, IGameSystem>> Systems =
            new Dictionary<string, Dictionary<SystemTypes, IGameSystem>>();

        private static readonly Dictionary<string, string> WorldComponents = new Dictionary<string, string>(8000);
        private static readonly Dictionary<string, string> Entities = new Dictionary<string, string>(8000);

        private static readonly Dictionary<string, SystemTypes> TypeLookUp =
            new Dictionary<string, SystemTypes>()
            {
                { typeof(BallSystem).Name,       SystemTypes.Ball },
                { typeof(BrickSystem).Name,      SystemTypes.Brick },
                { typeof(ColliderSystem).Name,   SystemTypes.Collider },
                { typeof(MoveableSystem).Name,   SystemTypes.Movable },
                { typeof(PlayerSystem).Name,     SystemTypes.Player },
                { typeof(PowerUpSystem).Name,    SystemTypes.PowerUp }
            };

        public static void Add(string worldName, IGameSystem system)
        {
            SystemTypes type = system.GetSystemType();

            if (!Systems.ContainsKey(worldName))
            {
                Systems.Add(worldName, new Dictionary<SystemTypes, IGameSystem>());
            }

            Systems[worldName].Add(type, system);
        }

        public static IGameComponent Get<T>(string component)
        {
            string worldName = WorldComponents[component];
            dynamic systems = Systems[worldName];
            string systemName = typeof(T).Name + "System";
            dynamic type = TypeLookUp[systemName];
            dynamic system = systems[type];
            return system.Get(component);
        }

        public static T GetSystem<T>(string worldName) where T : IGameSystem
        {
            SystemTypes systemType = TypeLookUp[typeof(T).Name];
            return (T)Systems[worldName][systemType];
        }

        public static void MapComponentWorld(string world, string component)
        {
            if (!WorldComponents.Contains(new KeyValuePair<string, string>(component, world)))
                WorldComponents.Add(component, world);
        }
        public static void MapComponentEntity(string world, string entity)
        {
            if (!Entities.Contains(new KeyValuePair<string, string>(entity, world)))
                Entities.Add(entity, world);
        }

        public static void Remove(SystemTypes type, string name) => Systems[WorldComponents[name]][type].Remove(name);
    }
}
