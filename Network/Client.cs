using System.Net.Sockets;

namespace PIGMServer.Network
{
    public class Client
    {
        public TcpClient tcpClient { get; private set; }
        public NetworkStream Stream { get; private set; }
        public ClientOwner Owner { get; private set; }

        public Client(TcpClient newClient)
        {
            tcpClient = newClient;
            Stream = tcpClient.GetStream();

        }

        public void HandedTo(ClientOwner processor)
        {
            Owner = processor;
        }

        public virtual void HandleMessage(Message message)
        {

        }
    }
}
