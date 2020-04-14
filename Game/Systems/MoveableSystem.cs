using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;

namespace PIGMServer.Game.Systems
{
    public class MoveableSystem : GameSystem<Movable>
    {
        public MoveableSystem(SubWorld world) : base(world)
        { }

        protected override void Process(Movable component, float deltaTime)
        {
            Vector2 position = component.Parent.Position;
            Vector2 velocity = component.Direction;

            component.Parent.Position = position + (position * (velocity * deltaTime));
        }

        protected override Message GatherAlterations(Movable alteredComponent)
        {
            return new Message(1, 1);
        }

        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Movable;
        }
    }
}
