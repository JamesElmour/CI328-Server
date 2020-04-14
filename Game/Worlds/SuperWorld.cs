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
        private int SliceCount;
        private int SubworldIndex = 0;
        private double TimeTillUpdate = 0;
        private DateTime PreviousDateTime;

        public SuperWorld(int tps = 60)
        {
            Tps = tps;
            DeltaTime = 1.0f / (float) tps;
            TimeTillUpdate = 1.0f / (float) Tps;
            PreviousDateTime = DateTime.Now;

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

        public void TransmitAtlerations(int priority)
        {
            MessageQueue queue = new MessageQueue();
            queue.Add(SubWorlds[SubworldIndex].GatherAlterations(priority));

            SendQueue(Get(SubworldIndex), queue);
        }

        #region World Slices and Updating
        /// <summary>
        /// Calculate the number of slices required.
        /// </summary>
        private void CalculateSliceCount()
        {
            int worldCount = SubWorlds.Count;
            SliceCount = (int) Math.Ceiling((float) worldCount / SubworldsPerFrame);
        }

        /// <summary>
        /// Create and assign a Slice for the required index.
        /// </summary>
        /// <param name="index">Start index of SubWorlds array to start assigning SubWorlds from.</param>
        /// <returns>Created WorldSlice</returns>
        private WorldSlice CreateSlice(int index)
        {
            int start = index * SubworldsPerFrame;
            int end   = (SubWorlds.Count < SubworldsPerFrame) ? SubWorlds.Count : SubworldsPerFrame;

            WorldSlice slice = new WorldSlice();
            slice.AddRange(SubWorlds.GetRange(start, end));

            return slice;
        }

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
            DateTime currentDateTime = DateTime.Now;
            TimeSpan differenceTimeSpan = currentDateTime - PreviousDateTime;
            PreviousDateTime = currentDateTime;
            double difference = differenceTimeSpan.TotalSeconds;
            TimeTillUpdate -= difference;

            if (TimeTillUpdate <= 0)
            {
                DeltaTime = (float) differenceTimeSpan.TotalSeconds;
                TimeTillUpdate = 1.0d / Tps;
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Update the next slice.
        /// </summary>
        private void UpdateWorld()
        {
            SubworldIndex++;
            if(SubworldIndex == SubWorlds.Count)
            {
                SubworldIndex = 0;
            }

            SubWorld sub = SubWorlds[SubworldIndex];
            Client client = Get(sub.WorldIndex);
            //SendQueue(client, sub.Update(DeltaTime));
            sub.Update(0.01666666f);
        }
        #endregion
    }
}