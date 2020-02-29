using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class Player : GameComponent
    {
        public Vector2 Dimensions;

        public Player(GameEntity parent) : base(parent)
        {

        }
    }
}
