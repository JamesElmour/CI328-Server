using PIGMServer.Game.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    public abstract class GameComponent : IGameComponent
    {
        public string Name { get { return Parent.Name; } }  // Name of the Game Component's parent Entity.
        public bool Altered = false;                        // If the Game Component has been altered.
        public bool IsJustCreated { get; private set; }     // Is the Game Component has just been created.
        public GameEntity Parent;                           // Game Component's parent.

        /// <summary>
        /// Create the Game Component with the given Game Entity.
        /// </summary>
        /// <param name="parent">Game Component's parent Entity.</param>
        public GameComponent(GameEntity parent)
        {
            Parent = parent;
            IsJustCreated = true;
        }

        /// <summary>
        /// Get if the Game Component has been altered.
        /// </summary>
        /// <returns>If the Game Component has been altered.</returns>
        public bool IsAltered()
        {
            bool altered = Altered;
            Altered = false;

            return altered;
        }

        /// <summary>
        /// Get the Component's name.
        /// </summary>
        /// <returns>Component's name.</returns>
        public string GetName()
        {
            return Name;
        }

        /// <summary>
        /// Sets Component to be old.
        /// </summary>
        public void IsOldNow()
        {
            IsJustCreated = false;
        }

        /// <summary>
        /// Returns if the Component has just been created.
        /// </summary>
        /// <returns>If the Component has just been created</returns>
        public bool JustCreated()
        {
            return IsJustCreated;
        }

        public GameEntity GameParent => Parent;

        public abstract SystemTypes GetSystem();
    }
}
