using System.Collections.Generic;

namespace PIGMServer.Network
{
    /// <summary>
    /// Owner of Client that contains functionality to interfact with TCP Clients.
    /// </summary>
    public abstract class ClientOwner
    {
        // Set Accept Limit to signal when full.
        protected int AcceptLimit = 2;
        protected int AcceptedClients = 0;
        public bool IsFull { get { return (AcceptedClients >= AcceptLimit); } } // Return if the Client Owner is full.

        public abstract void Give(Client client);
        public abstract void HandleMessage(Client client, Message message);
        public abstract void Remove(Client client);

        /// <summary>
        /// Check if the Client Owner can accept any more Clients.
        /// </summary>
        /// <returns>If the Client can accept more.</returns>
        public bool CanAcceptClient()
        {
            return (AcceptedClients < AcceptLimit);
        }
        
        /// <summary>
        /// Send a Message Queue to the Client.
        /// </summary>
        /// <param name="client">Client to send Messages to.</param>
        /// <param name="queue">Queue to send.</param>
        public void SendQueue(Client client, MessageQueue queue)
        {
            foreach(Message m in queue.Get())
                ClientAcceptor.SendMessage(client, m);
        }

    }
}
