using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game.Worlds
{
    public class OverWorld : GameClientHandler
    {
        public enum WorldStates
        {
            Initialising,
            Loading,
            Ready,
            Playing
        }

        private List<SubWorld> SubWorlds = new List<SubWorld>();
        private List<WorldSlice> Slices = new List<WorldSlice>();
        private readonly int Tps;
        private readonly float DeltaTime;
        private readonly int SubworldsPerFrame = 2;
        private int SliceCount;
        private int SliceUpdate = 0;

        public OverWorld(int tps = 20)
        {
            Tps = tps;
            DeltaTime = 1 / tps;


        }

        public void Start()
        {
            CalculateSliceCount();
            GenerateWorldSlices();
        }

        #region World Slices and Updating
        /// <summary>
        /// Calculate the number of slices required.
        /// </summary>
        private void CalculateSliceCount()
        {
            int worldCount = SubWorlds.Count;
            SliceCount = worldCount / SubworldsPerFrame;
        }

        /// <summary>
        /// Generate the slices fo rthe OverWorld, assigning SubWorlds to them for updating.
        /// </summary>
        private void GenerateWorldSlices()
        {            
            for(int i = 0; i < SliceCount; i++)
            {
                Slices.Add(CreateSlice(i));
            }
        }

        /// <summary>
        /// Create and assign a Slice for the required index.
        /// </summary>
        /// <param name="index">Start index of SubWorlds array to start assigning SubWorlds from.</param>
        /// <returns>Created WorldSlice</returns>
        private WorldSlice CreateSlice(int index)
        {
            int start = index * SubworldsPerFrame;

            WorldSlice slice = new WorldSlice();
            slice.AddRange(SubWorlds.GetRange(start, SubworldsPerFrame));

            return slice;
        }
        
        /// <summary>
        /// Update the OverWorld.
        /// </summary>
        public void Update()
        {
            UpdateSlice();
        }
        
        /// <summary>
        /// Update the next slice.
        /// </summary>
        private void UpdateSlice()
        {
            Slices[SliceUpdate].Update(DeltaTime);
            
            SliceUpdate++;
            if(SliceUpdate == SliceCount)
            {
                SliceUpdate = 0;
            }
        }
        #endregion
    }
}