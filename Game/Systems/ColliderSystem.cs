using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Network;

namespace PIGMServer.Game.Systems
{
    public class ColliderSystem : GameSystem<Collider>
    {
        public ColliderSystem(string worldName) : base(worldName)
        { }

        protected override void Preprocess(Collider component)
        {
            component.CollidingWith.Clear();
            component.CollidedComponents.Clear();
            component.Colliding = false;
        }

        protected override void Process(Collider component, float deltaTime)
        {
            if (!component.Active)
                return;

            foreach (Collider other in TemporaryComponents)
            {
                if (Collides(component, other))
                {
                    ExecuteCollision(component, other);
                }
            }
        }
        private void ExecuteCollision(Collider component, Collider other)
        {
            string tag = other.Parent.Tag;
            component.Colliding = true;
            other.Colliding = true;

            if (tag.Equals("Brick"))
            {
                int a = 2;
            }

            if (!component.CollidingWith.Contains(tag))
            {
                component.CollidingWith.Add(tag);
                component.CollidedComponents.Add(other);

                other.CollidingWith.Add(component.Parent.Tag);
                other.CollidedComponents.Add(component);
            }
        }

        private bool Collides(Collider a, Collider b)
        {

            return a != b && Rectangle.Intersects(a.Rect, b.Rect);
        }
        public override SystemTypes GetSystemType()
        {
            return SystemTypes.Collider;
        }

        protected override Message GatherAlterations(Collider alteredComponent)
        {
            return new Message(1, 1);
        }
    }
}
