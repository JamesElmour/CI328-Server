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
        Powerup,
        Unknown
    }

    public static class SystemManager
    {
        [ThreadStatic]  // One instance per thread.
        private static readonly Dictionary<SystemTypes, IGameSystem> Systems =
            new Dictionary<SystemTypes, IGameSystem>();

        public static void Add(IGameSystem system, SystemTypes type)
        {
            Type systemType = system.GetType();
            if (Systems.ContainsKey(type))
            {
                Systems[type].Clear();
            }
            else
            {
                Systems.Add(type, system);
            }
        }

        public static void Clear(SystemTypes type)
            => Systems[type].Clear();

        public static IGameComponent GetComponent(SystemTypes systemType, string component)
        {
            GameSystem<IGameComponent> system =
                (GameSystem<IGameComponent>) Systems[systemType];
            return system.GetComponent(component);
        }

        public static Type FindSystemType(SystemTypes systemType)
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
                    // TODO: Replace
                    throw new Exception("Player System not implemented.");
                case SystemTypes.Powerup:
                    throw new Exception("Powerup System not implemented.");
                default:
                    throw new Exception("Invalid System Type provided.");
            }

        }
    }
}
