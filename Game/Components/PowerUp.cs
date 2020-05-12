using PIGMServer.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    /// <summary>
    /// Component which houses PowerUp data for ECS technique.
    /// </summary>
    public class PowerUp : GameComponent
    {
        public bool Used = false;       // If the PowerUp has been used.

        // Enum of all PowerUps.
        public enum PowerUps
        {
            SpeedBall,
            MultiBall,
            RapidBall,
            QuadBall,
            ExendPlayer,
            ShrinkPlayer,
            BeefyBricks,
            Invincibility,
            ExtraLife
        }
        public readonly PowerUps Type; // Current PowerUp's type.
        public float TimeTillEnd;      // How long till PowerUp ends.

        /// <summary>
        /// Creates PowerUp of given PowerUps type.
        /// </summary>
        /// <param name="parent">PowerUp's parent Entity.</param>
        /// <param name="type">Type of PowerUp.</param>
        public PowerUp(GameEntity parent, PowerUps type) : base(parent)
        {
            Type = type;
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Ball;
        }
    }
}
