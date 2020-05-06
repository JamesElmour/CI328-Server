using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    public interface IGameComponent
    {
        GameEntity GetParent { get; }
        bool IsAltered();
        string GetName();

        void IsOldNow();
        bool JustCreated();
    }
}
