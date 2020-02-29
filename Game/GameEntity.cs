using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PIGMServer.Game.Types;

namespace PIGMServer.Game
{
    public class GameEntity
    {
        public string Name { get; protected set; }
        public string Tag { get; protected set; }
        private Dictionary<string, Type> Components = new Dictionary<string, Type>();
        public Vector2 Position;

        public GameEntity(string name, Vector2 position = null, string tag = "")
        {
            if (position == null)
                position = new Vector2(0);

            Name = name;
            Position = position;
            Tag = tag;
        }

        public void AddComponent(GameComponent component) => Components.Add(component.Name, component.System);
        public void RemoveComponent(GameComponent component) => Components.Remove(component.Name);
    }
}
