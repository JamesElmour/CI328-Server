using System.Collections.Generic;
using System.Threading;

namespace PIGMServer.Game.Worlds
{
    public abstract class SuperWorld<T> : GameClientHandler where T : SubWorld
    {
        public bool Destroy = false;
        protected List<T> SubWorlds = new List<T>();
        //private List<WorldSlice> Slices = new List<WorldSlice>();
        private readonly int Tps;
        private float DeltaTime;
        private readonly int SubworldsPerFrame = 1;
        protected int SubworldIndex = 0;

        public SuperWorld(int tps = 20)
        {
            Tps = tps;
            DeltaTime = 1.0f / (float)tps;

            Start();
        }

        public void Start()
        {
            CreateSubWorlds();
            //CalculateSliceCount();
            //GenerateWorldSlices();
        }

        /// <summary>
        /// Add SubWorld to the OverWorld.
        /// </summary>
        /// <param name="world">SubWorld to be added.</param>
        protected virtual void AddSubWorld(T world)
        {
            SubWorlds.Add(world);
        }

        protected abstract void CreateSubWorlds();

        public abstract void TransmitAtlerations(int priority);

        /// <summary>
        /// Update the OverWorld.
        /// </summary>
        public void Update()
        {
            if (IsFull && ShouldUpdate())
            {
                if (Clients.Count == AcceptLimit)
                {
                    UpdateWorld();
                    TransmitAtlerations(1);
                }
                else
                {
                    Destroy = true;
                }
            }
        }

        private bool ShouldUpdate()
        {
            double TimeTillUpdate = 1.0d / Tps / 2;
            Thread.Sleep((int)(TimeTillUpdate * 500));
            return true;
        }

        /// <summary>
        /// Update the next slice.
        /// </summary>
        private void UpdateWorld()
        {
            SubworldIndex++;
            if (SubworldIndex == SubWorlds.Count)
            {
                SubworldIndex = 0;
            }

            SubWorld sub = SubWorlds[SubworldIndex];
            //Client client = Get(sub.WorldIndex);
            //SendQueue(client, sub.Update(DeltaTime));
            sub.Update(0.01666666f);
        }
    }
}