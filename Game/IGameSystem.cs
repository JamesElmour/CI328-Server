using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    public interface IGameSystem
    {
        void Update(float deltaTime);
        void Clear();
        public void Remove(string name)
    }
}
