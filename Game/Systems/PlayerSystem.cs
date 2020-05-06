using PIGMServer.Game.Components;
using PIGMServer.Game.Worlds;
using PIGMServer.Game.Worlds.Levels;
using PIGMServer.Network;
using PIGMServer.Utilities;
using System;


namespace PIGMServer.Game.Systems
{
    public class PlayerSystem : GameSystem<Player>
    {
        public PlayerSystem(SubWorld world) : base(world)
        { }

        protected override void Process(Player component, float deltaTime)
        {
            component.Direction = (ushort) Math.Max(Math.Min(component.Direction, (short)(2)), (short)(0));

            GameEntity parent = component.Parent;
            ushort speed = component.Speed;
            short direction = (short)(component.Direction - 1);
            short velocity = (short)(direction * speed * deltaTime);

            parent.Position.x += velocity;
            component.Altered = true;

            parent.Position.x = Math.Max(Math.Min(parent.Position.x, (short)(1280)), (short)(0));

            if (component.Lives <= 0)
                ((BreakoutWorld) World).Dead = true;
        }

        protected override Message GatherAlterations(Player alteredComponent)
        {
            short newPosition = alteredComponent.Parent.Position.x;
            short superOp = (short)SuperOps.Player;
            short subOp = (short)PlayerOps.PositionUpdate;
            byte[] byteData = Util.GetBytes(newPosition, 2);

            return new Message(superOp, subOp, byteData);
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Player;
        }
    }
}
