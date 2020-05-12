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
    /// <summary>
    /// Main Server class to start the game.
    /// </summary>
    class Server
    {
        static void Main(string[] args)
        {
            // Set IP and port, then start listening to it.
            string ip = "0.0.0.0";
            int port = 8111;
            TcpListener server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Console.WriteLine("Server has started on {0}:{1}, Waiting for a connection...", ip, port);

            // Create thread to manage Client connection.
            Thread managerThread = new Thread(() =>
            {
                // Set up Client Accept's Owner Creation Function.
                ClientAcceptor.OwnerCreationFunc = () =>
                {
                    // Create a new Thread for the Client Owner.
                    Thread worldThread = new Thread(() =>
                    {
                        // Create Breakout world and queur it.
                        BreakoutSuperWorld world = new BreakoutSuperWorld();
                        ClientAcceptor.QueueOwner(world);

                        // Keep updating world till it's destroyed.
                        while (!world.Destroy)
                        {
                            world.Update();
                        }

                        ClientAcceptor.OwnerCreationFunc();
                    });

                    // Name and start world's thread.
                    worldThread.Name = "World Thread " + new Guid().ToString();
                    worldThread.Start();

                    return 1;
                };

                // Create Owner to accept.
                ClientAcceptor.OwnerCreationFunc();

                // Start listening for new Clients.
                ClientAcceptor.Start(server);
            });

            // Name and start main manager thread.
            managerThread.Name = "Manager Thread";
            managerThread.Start();
        }
    }
}
