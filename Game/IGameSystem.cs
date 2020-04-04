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
        void Update(float deltaTime);
        void Clear();
        void Remove(string name);
        //IGameComponent Get(string name);
        SystemTypes GetSystemType();
        int GetPriority();
        List<Message> GetAlterations();
    }
}
