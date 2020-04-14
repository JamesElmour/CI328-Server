using PIGMServer.Game.Components;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;

namespace PIGMServer.Game.Systems
{
    class PowerUpSystem : GameSystem<PowerUp>
    {
        public PowerUpSystem(SubWorld world) : base(world)
        { }

        protected override void Process(PowerUp component, float deltaTime)
        {

        }

        protected override Message GatherAlterations(PowerUp alteredComponent)
        {
            return new Message(1, 1);
        }


        public override SystemTypes GetSystemType()
        {
            return SystemTypes.PowerUp;
        }
    }
}
