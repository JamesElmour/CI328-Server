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
        public      string      Name        { get; private set; }
        public      bool        Altered =   false;
        public      GameEntity  Parent;
        public      SystemTypes System  = SystemTypes.Unknown;
        protected   List<KeyValuePair<string, byte[]>> Alterations = new List<KeyValuePair<string, byte[]>>();

        public GameComponent(GameEntity parent)
        {
            Parent = parent;
            GenerateName();
        }
        private void GenerateName()
        {
            Guid id = Guid.NewGuid();
            Name = id.ToString();
        }

        public virtual void ClearAlterations()
        {
            if (Altered)
            {
                Alterations.Clear();
                Altered = false;
            }
        }
         
        public bool IsAltered() => Altered;
        public List<KeyValuePair<string, byte[]>> GetAlterations() => Alterations;
        public GameEntity GetParent => Parent;
    }
}
