using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds
{
    public abstract class OverWorld : GameClientHandler
    {
        public enum WorldStates
        {
            Initialising,
            Loading,
            Ready,
            Playing
        }

        private List<SubWorld> SubWorlds = new List<SubWorld>();
        //private List<WorldSlice> Slices = new List<WorldSlice>();
        private readonly int Tps;
        private readonly float DeltaTime;
        private readonly int SubworldsPerFrame = 1;
        private int SliceCount;
        private int SubworldIndex = 0;
        private double TimeTillUpdate = 0;
        private DateTime PreviousDateTime;

        public OverWorld(int tps = 10)
        {
            Tps = tps;
            DeltaTime = 1.0f / tps;
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
        protected void AddSubWorld(SubWorld world)
        {
            SubWorlds.Add(world);
        }

        protected abstract void CreateSubWorlds();

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
        /// Generate the slices fo rthe OverWorld, assigning SubWorlds to them for updating.
        /// </summary>
        /*private void GenerateWorldSlices()
        {            
            for(int i = 0; i < SliceCount; i++)
            {
                Slices.Add(CreateSlice(i));
            }
        }*/

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
            if(IsFull && ShouldUpdate())
                UpdateWorld();
        }

        private bool ShouldUpdate()
        {
            DateTime currentDateTime = DateTime.Now;
            TimeSpan differenceTimeSpan = currentDateTime - PreviousDateTime;
            PreviousDateTime = currentDateTime;
            double difference = differenceTimeSpan.TotalMilliseconds / 1000.0f;
            TimeTillUpdate -= difference;

            if (TimeTillUpdate <= 0)
            {
                TimeTillUpdate = 1.0d / (double) Tps;
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
            SendQueue(client, sub.Update(DeltaTime));
        }
        #endregion
    }
}