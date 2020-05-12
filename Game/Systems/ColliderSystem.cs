using PIGMServer.Game.Components;
using PIGMServer.Game.Types;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;

namespace PIGMServer.Game.Systems
{
    /// <summary>
    /// System which processes all Collider for ECS technique.
    /// </summary>
    public class ColliderSystem : GameSystem<Collider>
    {
        /// <summary>
        /// Create Collider system.
        /// </summary>
        /// <param name="world">Owning SubWorld.</param>
        public ColliderSystem(SubWorld world) : base(world)
        { }

        /// <summary>
        /// Preprocess the given Collider.
        /// </summary>
        /// <param name="component">Given Collider.</param>
        protected override void Preprocess(Collider component)
        {
            // Clear all colliding with lists, and set not colliding.
            component.CollidingWith.Clear();
            component.CollidedComponents.Clear();
            component.Colliding = false;
        }

        /// <summary>
        /// Process the given Collider.
        /// </summary>
        /// <param name="component">Given Collider.</param>
        /// <param name="deltaTime">Curent DeltaTime.</param>
        protected override void Process(Collider component, float deltaTime)
        {
            // Exit if the Collider isn't active.
            if (!component.Active)
                return;

            // Otherwise with each other Collider...
            foreach (Collider other in TemporaryComponents)
            {
                if (Collides(component, other)) // ... If Collider has collided...
                {
                    // ... Execute collision.
                    ExecuteCollision(component, other);
                }
            }
        }

        /// <summary>
        /// Execute collision with current Collider and other Collider.
        /// </summary>
        /// <param name="component">Current Collider.</param>
        /// <param name="other">Other Collider.</param>
        private void ExecuteCollision(Collider component, Collider other)
        {
            // Get other Collider's tag and set both Colliders to colliding.
            string tag = other.Parent.Tag;
            component.Colliding = true;
            other.Colliding = true;

            if (!component.CollidingWith.Contains(tag)) // If current Collider doesn't contain other's tag...
            {
                // ... Give tags and Collider to current and other's Collider.
                component.CollidingWith.Add(tag);
                component.CollidedComponents.Add(other);

                other.CollidingWith.Add(component.Parent.Tag);
                other.CollidedComponents.Add(component);
            }
        }

        /// <summary>
        /// Detect if Collider A and B are colliding.
        /// </summary>
        /// <param name="a">Collider A</param>
        /// <param name="b">Collider B</param>
        /// <returns></returns>
        private bool Collides(Collider a, Collider b)
        {
            // Check A and B aren't the same, check their Rectangles intersect.
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
