using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Network
{
    public abstract class ClientOwner
    {
        protected int AcceptLimit = 4;
        protected int AcceptedClients = 0;

        public abstract void Give(Client client);
        public abstract void HandleMessage(Client client, Message message);
        public bool CanAcceptClient()
        {
            return (AcceptedClients <= AcceptLimit);
        }
    }
}
