using PIGMServer.Game.Systems;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    public interface IGameSystem
    {
        List<Message> Update(float deltaTime);
        void Clear();
        void Remove(string name);
        //IGameComponent Get(string name);
        SystemTypes GetSystemType();
    }
}
