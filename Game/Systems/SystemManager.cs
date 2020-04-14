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
        // One instance per thread.
        private static readonly ThreadLocal<Dictionary<string, Dictionary<SystemTypes, IGameSystem>>> Systems = new ThreadLocal<Dictionary<string, Dictionary<SystemTypes, IGameSystem>>>
            (() => { return new Dictionary<string, Dictionary<SystemTypes, IGameSystem>>(); });

        // One instance per thread.
        private static readonly ThreadLocal<Dictionary<string, string>> WorldComponents = new ThreadLocal<Dictionary<string, string>>
        (() => { return new Dictionary<string, string>(8000); });

        // One instance per thread.
        private static readonly ThreadLocal<Dictionary<string, string>> Entities = new ThreadLocal<Dictionary<string, string>>
        (() => { return new Dictionary<string, string>(8000); });

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

            if (!Systems.Value.ContainsKey(worldName))
            {
                Systems.Value.Add(worldName, new Dictionary<SystemTypes, IGameSystem>());
            }

            Systems.Value[worldName].Add(type, system);
        }

        public static IGameComponent Get<T>(string component)
        {
            string worldName = WorldComponents.Value[component];
            dynamic systems = WorldComponents.Value[worldName];
            string systemName = typeof(T).Name + "System";
            dynamic type = TypeLookUp[systemName];
            dynamic system = systems[type];
            return system.Get(component);
        }

        public static T GetSystem<T>(string worldName) where T : IGameSystem
        {
            SystemTypes systemType = TypeLookUp[typeof(T).Name];
            return (T)Systems.Value[worldName][systemType];
        }

        public static void MapComponentWorld(string world, string component)
        {
            if (!WorldComponents.Value.Contains(new KeyValuePair<string, string>(component, world)))
                WorldComponents.Value.Add(component, world);
        }
        public static void MapComponentEntity(string world, string entity)
        {
            if (!Entities.Value.Contains(new KeyValuePair<string, string>(entity, world)))
                Entities.Value.Add(entity, world);
        }

        public static void Remove(SystemTypes type, string name) => Systems.Value[WorldComponents.Value[name]][type].Remove(name);
    }
}
