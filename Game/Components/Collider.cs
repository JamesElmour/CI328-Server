using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{
    public class Collider : GameComponent
    {
        public Rectangle Rect { get { return new Rectangle(Parent.Position, Width, Height); } }
        public bool Active;
        public bool Colliding;
        public List<string> CollidingWith = new List<string>(5);
        public List<IGameComponent> CollidedComponents = new List<IGameComponent>(5);
        public new SystemTypes System = SystemTypes.Collider;
        public short Width, Height;

        public Collider(GameEntity parent, short width, short height, bool active = false) : base(parent)
        {
            Width = width;
            Height = height;
            Active = active;
        }

        public override SystemTypes GetSystem()
        {
            return SystemTypes.Collider;
        }
    }
}
