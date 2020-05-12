using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PIGMServer.Game.Systems
{
    /// <summary>
    /// Types of all System types in the game.
    /// </summary>
    public enum SystemTypes
    {
        Ball,
        Brick,
        Collider,
        Movable,
        Player,
        PowerUp,
        Unknown
    }

    // TODO: Redundant file from previous System Manager implementation which was removed, refactor this.
}
