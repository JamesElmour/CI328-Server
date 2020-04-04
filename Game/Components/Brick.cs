using PIGMServer.Game.Systems;
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
        public readonly short X, Y;
        public int Health = 3;
        public new SystemTypes System = SystemTypes.Brick;

        public Brick(GameEntity parent, int x, int y) : base(parent)
        {
            X = (short) x;
            Y = (short) y;
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Brick;
        }
    }
}
