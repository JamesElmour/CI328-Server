using PIGMServer.Game.Systems;
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
        void Remove(string name);
        SystemTypes GetSystemType();
    }
}
