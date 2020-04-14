using System.Collections.Generic;

namespace PIGMServer.Network
{
    public abstract class ClientOwner
    {
        protected int AcceptLimit = 1;
        protected int AcceptedClients = 0;
        public bool IsFull { get { return (AcceptLimit == AcceptedClients); } }

        public abstract void Give(Client client);
        public abstract void HandleMessage(Client client, Message message);
        public bool CanAcceptClient()
        {
            return (AcceptedClients <= AcceptLimit);
        }

        public void SendQueue(Client client, MessageQueue queue)
        {
            List<Message> messages = queue.Get();

            foreach(Message message in messages)
            {
                ClientAcceptor.SendMessage(client, message);
            }
        }
        public abstract void Remove(Client client);
    }
}
