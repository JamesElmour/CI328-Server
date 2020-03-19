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
        [ThreadStatic]  // One instance per thread.
        private static readonly Dictionary<SystemTypes, IGameSystem> Systems =
            new Dictionary<SystemTypes, IGameSystem>();

        public static void Add(IGameSystem system)
        {
            Type systemType = system.GetType();
            SystemTypes type = system.GetSystemType();

            if (Systems.ContainsKey(type))
            {
                Systems[type].Clear();
            }
            else
            {
                Systems.Add(type, system);
            }
        }

        public static IGameComponent Get(SystemTypes systemType, string component)
        {
            GameSystem<IGameComponent> system =
                (GameSystem<IGameComponent>) Systems[systemType];
            return system.Get(component);
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
        
        public static void Clear(SystemTypes type)
            => Systems[type].Clear();
        public static void Remove(SystemTypes type, string name) => Systems[type].Remove(name);
    }
}
