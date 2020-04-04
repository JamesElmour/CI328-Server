using PIGMServer.Game;
using PIGMServer.Game.Worlds;
using PIGMServer.Game.Worlds.Levels;
using PIGMServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PIGMServer
{
    class Server
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 80;
            TcpListener server = new TcpListener(IPAddress.Parse(ip), port);

            server.Start();
            Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection...", ip, port);

            Thread managerThread = new Thread(() =>
            {
                ClientAcceptor.OwnerCreationFunc = () =>
                {
                    Thread worldThread = new Thread(() =>
                    {
                        BreakoutSuperWorld testWorld = new BreakoutSuperWorld();
                        ClientAcceptor.QueueOwner(testWorld);

                        while (!testWorld.Destroy)
                        {
                            testWorld.Update();
                        }

                        ClientAcceptor.OwnerCreationFunc();
                    });
                    worldThread.Start();

                    return 1;
                };
                ClientAcceptor.OwnerCreationFunc();
                ClientAcceptor.Start(server);
            });
            managerThread.Start();
        }
    }
}
