using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Game
{
    public interface IGameComponent
    {
        bool IsAltered();
        List<KeyValuePair<string, byte[]>> GetAlterations();
    }
}
