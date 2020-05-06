using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Types;

namespace PIGMServer.Game
{
    public class GameEntity
    {
        public string Name { get; protected set; }
        public string Tag { get; protected set; }
        private Dictionary<SystemTypes, string> Components = new Dictionary<SystemTypes, string>();
        public Vector2 Position;
        public bool Destroy = false;

        public GameEntity(string name, Vector2 position = null, string tag = "")
        {
            if (position == null)
                position = new Vector2(0);

            Name = name;
            Position = position;
            Tag = tag;
        }

        public void Add(GameComponent component) => Components.Add(component.GetSystem(), component.Name);
        public void Add(IGameComponent component) => Add((GameComponent) component);

        public string Get(SystemTypes type) => Components[type];
    }
}
