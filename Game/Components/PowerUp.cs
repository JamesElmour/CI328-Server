using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class PowerUp : GameComponent
    {
        public enum Types
        {
            Multiball,
            Incinvibility,
            Fastball,
            WidePlayer,
            NarrowPlayer
        }
        public Types Type;

        public PowerUp(GameEntity parent, Types type) : base(parent)
        {
            Type = type;
        }

        public int TypeValue()
        {
            return (int) Type;
        }
    }
}
