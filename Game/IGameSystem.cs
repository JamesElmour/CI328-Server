using PIGMServer.Game.Systems;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    /// <summary>
    /// Interface for all Game Systems.
    /// </summary>
    public interface IGameSystem
    {
        /// <summary>
        /// Update the System with DeltaTime.
        /// </summary>
        /// <param name="deltaTime">DeltaTime.</param>
        void Update(float deltaTime);

        SystemTypes GetSystemType();

        /// <summary>
        /// Get System's priority.
        /// </summary>
        /// <returns>Returns priority.</returns>
        int GetPriority();

        /// <summary>
        /// Get alterations of System's Components.
        /// </summary>
        /// <returns>List of Messages containing alterations.</returns>
        List<Message> GetAlterations();
    }
}
