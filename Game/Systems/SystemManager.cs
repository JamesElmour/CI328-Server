using PIGMServer.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Systems
{
    public enum SystemTypes
    {
        Ball,
        Brick,
        Collide,
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

        private static readonly Dictionary<string, string> componentWorlds = new Dictionary<string, string>(8000);

        private static readonly Dictionary<Type, SystemTypes> TypeLookUp =
            new Dictionary<Type, SystemTypes>()
            {
                { typeof(BallSystem),       SystemTypes.Ball },
                { typeof(BrickSystem),      SystemTypes.Brick },
                { typeof(ColliderSystem),   SystemTypes.Collide },
                { typeof(MoveableSystem),   SystemTypes.Movable },
                { typeof(PlayerSystem),     SystemTypes.Player },
                { typeof(PowerUpSystem),    SystemTypes.PowerUp }
            };

        public static void Add(string worldName, IGameSystem system)
        {
            SystemTypes type = system.GetSystemType();

            if(!Systems.ContainsKey(worldName))
            {
                Systems.Add(worldName, new Dictionary<SystemTypes, IGameSystem>());
            }

            Systems[worldName].Add(type, system);
        }

        public static IGameComponent Get<T>(string component)
        {
            string worldName = componentWorlds[component];
            dynamic system   = Systems[worldName][TypeLookUp[typeof(T)]];
            return system.Get(component);
        }

        public static T GetSystem<T>(string worldName) where T : IGameSystem
        {
            SystemTypes systemType = TypeLookUp[typeof(T)];
            return (T) Systems[worldName][systemType];
        }

        public static Type GetSystemClassFromType(SystemTypes systemType)
        {
            switch (systemType)
            {
                case SystemTypes.Ball:
                    return typeof(BallSystem);
                case SystemTypes.Brick:
                    return typeof(BrickSystem);
                case SystemTypes.Collide:
                    return typeof(ColliderSystem);
                case SystemTypes.Movable:
                    return typeof(MoveableSystem);
                case SystemTypes.Player:
                    return typeof(PlayerSystem);
                case SystemTypes.PowerUp:
                    return typeof(PowerUpSystem);
                default:
                    throw new Exception("Invalid System Type provided.");
            }
        }

        public static void MapComponentWorld(string world, string component) => componentWorlds.Add(component, world);

        public static void Remove(SystemTypes type, string name) => Systems[componentWorlds[name]][type].Remove(name);
    }
}
