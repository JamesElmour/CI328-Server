using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

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
