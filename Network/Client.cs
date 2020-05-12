using System.Net.Sockets;

namespace PIGMServer.Network
{
    /// <summary>
    /// Client containing all information about a connected TCP client.
    /// </summary>
    public class Client
    {
        public TcpClient tcpClient { get; private set; }    // Connected TCP Client.
        public NetworkStream Stream { get; private set; }   // Client's Stream.
        public ClientOwner Owner { get; private set; }      // Owner of this Client.
        public bool Connected = false;                      // If the Client is connected.

        /// <summary>
        /// Create Client with the provided TCP Client.
        /// </summary>
        /// <param name="newClient">The TCP Client the Client is representing.</param>
        public Client(TcpClient newClient)
        {
            tcpClient = newClient;
            Stream = tcpClient.GetStream();

        }

        /// <summary>
        /// Hand the Client to a Client Owner.
        /// </summary>
        /// <param name="processor">Client's Client Owner.</param>
        public void HandedTo(ClientOwner processor)
        {
            Owner = processor;
        }
    }
}
