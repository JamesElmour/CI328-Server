using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game
{
    /// <summary>
    /// Game Entity which houses components.
    /// </summary>
    public class GameEntity
    {
        public string Name { get; protected set; }  // Entity's name.
        public string Tag { get; protected set; }   // Entity's tag.
        public Vector2 Position;                    // Current position in worldspace.
        public bool Destroy = false;                // If the Entity should be destoryed.

        /// <summary>
        /// Create the Entiy with the given; name, position, and tag.
        /// </summary>
        /// <param name="name">Desired name for the Entity.</param>
        /// <param name="position">Position of the Entity in worldspace.</param>
        /// <param name="tag">Tag for the Entity.</param>
        public GameEntity(string name, Vector2 position = null, string tag = "")
        {
            // If the position is null, set to 0.
            if (position == null)
                position = new Vector2(0);

            // Set up attributes.
            Name = name;
            Position = position;
            Tag = tag;
        }
    }
}
