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
                    component.Colliding = true;
                    component.CollidingWith.Add()
                }
            }
        }

        private bool Collides(Collider a, Collider b)
        {
            return Rectangle.Intersects(a.Rect, b.Rect);
        }
    }
}
