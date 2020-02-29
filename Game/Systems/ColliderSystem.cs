using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Systems
{
    public class ColliderSystem : GameSystem<Collider>
    {
        protected override void Preprocess(Collider component)
        {
            component.CollidingWith.Clear();
            component.Colliding = false;
        }

        protected override void Process(Collider component, float deltaTime)
        {
            if (!component.Active)
                return;
                
            foreach(Collider other in TemporaryComponents)
            {
                if(Collides(component, other))
                {
                    ExecuteCollision(component, other);
                }
            }
        }
        private void ExecuteCollision(Collider component, Collider other)
        {
            string tag = other.Parent.Tag;
            component.Colliding = true;

            if(!component.CollidingWith.Contains(tag))
                component.CollidingWith.Add(tag);
        }

        private bool Collides(Collider a, Collider b)
        {
            return Rectangle.Intersects(a.Rect, b.Rect);
        }
    }
}
