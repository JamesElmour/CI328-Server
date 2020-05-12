using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Components
{

    /// <summary>
    /// Component which houses Collider data for ECS technique.
    /// </summary>
    public class Collider : GameComponent
    {
        public Rectangle Rect { get { return new Rectangle(Parent.Position, Width, Height); } }  // Getter which generates a Rectangle for the Collider.
        public bool Active;                                                                      // If this Collider is Active or Static.
        public bool Colliding;                                                                   // If this Collider has collided with something.
        public List<string> CollidingWith = new List<string>(5);                                 // List of tags which the Collider touched.
        public List<IGameComponent> CollidedComponents = new List<IGameComponent>(5);            // List of Components which the Collider touched.
        public short Width, Height;                                                              // Width and Height of the Collider.

        /// <summary>
        /// Create the Collider component.
        /// </summary>
        /// <param name="parent">Collider's parent Entity.</param>
        /// <param name="width">Width of Collider.</param>
        /// <param name="height">Height of Collider.</param>
        /// <param name="active">If Collider is active.</param>
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
