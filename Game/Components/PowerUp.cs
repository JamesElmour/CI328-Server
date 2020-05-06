using PIGMServer.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class PowerUp : GameComponent
    {
        public enum PowerUps
        {
            SpeedBall,
            MultiBall,
            RapidBall,
            QuadBall,
            ExendPlayer,
            ShrinkPlayer,
            BeefyBricks,
            Invincibility
        }
        public readonly PowerUps Type;
        public float TimeTillEnd;

        public bool JustCreated = true;

        public PowerUp(GameEntity parent, PowerUps type) : base(parent)
        {
            Type = type;
        }

        public int TypeValue()
        {
            return (int) Type;
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Ball;
        }
    }
}
