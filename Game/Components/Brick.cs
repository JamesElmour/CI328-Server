using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class Brick : GameComponent
    {
        private static readonly Vector2 Dimensions = new Vector2(64, 24);
        public int Health = 3;

        public Brick(GameEntity parent) : base(parent)
        {

        }
    }
}
