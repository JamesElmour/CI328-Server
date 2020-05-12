using PIGMServer.Network;
using PIGMServer.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace PIGMServer.Game.Worlds
{
    public abstract class SuperWorld<T> : GameClientHandler where T : SubWorld
    {
        public new bool IsFull { get { return SubWorlds.Count >= AcceptLimit; } } // Getter returns if SuperWorld is full of SubWorlds.
        public bool Destroy = false;                                              // If this SuperWorld should destroy.
        protected List<T> SubWorlds = new List<T>();                              // All SubWorlds added to the SuperWorld.
        private readonly int Tps;                                                 // Number of ticks per second the SuperWorld should run at.
        private float DeltaTime;                                                  // Current DeltaTime.
        protected bool HasEnded = false;                                          // If the SuperWorld has ended.
        protected T CurrentWorld = null;                                          // Current SubWorld to process.

        /// <summary>
        /// Create the SuperWorld with the given ticks-per-second.
        /// </summary>
        /// <param name="tps">Number of ticks-per-second the SuperWorld should run at.</param>
        public SuperWorld(int tps = 60)
        {
            // Calculate the number of ticks per second.
            Tps = tps;
            DeltaTime = 1.0f / (float)tps;

            // Prepare SuperWorld.
            PrepareWorld();
        }

        /// <summary>
        /// Add SubWorld to the OverWorld.
        /// </summary>
        /// <param name="world">SubWorld to be added.</param>
        protected virtual void AddSubWorld(T world, Client client)
        {
            SubWorlds.Add(world);
            world.Client = client;
        }

        protected abstract void PrepareWorld();

        public abstract void TransmitAtlerations(int priority);

        /// <summary>
        /// Update the OverWorld.
        /// </summary>
        public virtual void Update()
        {
            if (!HasEnded && Sleep() && IsFull) // If SuperWorld is ready...
            {
                if (CurrentWorld != null && Clients.Count == AcceptLimit) // ... And there are active players...
                {
                    // Reset SubWorld's gather alteraion priority to 0 if over 3.
                    CurrentWorld.GatherPriority = (CurrentWorld.GatherPriority == 3) ? 0 : CurrentWorld.GatherPriority;

                    // Gather alterations from SubWorld systems under the current priority.
                    TransmitAtlerations(CurrentWorld.GatherPriority++);
                    
                    // TransmitAtlerations(3); // Get all System Alterations - testing only!

                    // Update the SubWorld.
                    UpdateWorld();
                }
                else // ... Else if no active players...
                {
                    // End match.
                    Destroy = true;
                }
            }
        }

        /// <summary>
        /// Sleep the thread in accordance to ticks-per-second.
        /// </summary>
        /// <returns>True when finished sleeping.</returns>
        protected bool Sleep()
        {
            // Calculate time between ticks.
            double TimeTillUpdate = 1.0d / Tps;

            // Calculate current sleep time in milliseconds respective to number of SubWorlds.
            int sleepTime  = (int) (TimeTillUpdate * 1000);
                sleepTime /= Math.Max(SubWorlds.Count, 1);
                sleepTime  = Math.Max(sleepTime, 0);

            // Sleep thread.
            Thread.Sleep(sleepTime);
            return true;
        }

        /// <summary>
        /// Update the current SubWorld.
        /// </summary>
        protected virtual void UpdateWorld()
        {
            if (IsFull) // If the SuperWorld is full...
            {
                // ... Update the current SubWorld.
                CurrentWorld.Update(DeltaTime);
                PostUpdate(CurrentWorld);

                // Set current SubWorld to the current opponent.
                CurrentWorld = (T)CurrentWorld.Opponent;
            }
        }

        /// <summary>
        /// Process the given SubWorld after updating.
        /// </summary>
        /// <param name="world">Given SubWorld</param>
        protected virtual void PostUpdate(T world) { }
    }
}