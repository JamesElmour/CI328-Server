using PIGMServer.Game.Components;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game
{
    public class GameClientHandler : ClientOwner
    {
        private readonly List<Client> Clients;


        public GameClientHandler()
        {
            Clients = new List<Client>(AcceptLimit);
        }

        public override void Give(Client client)
        {
            Clients.Add(client); 
        }

        public override void HandleMessage(Client client, Message message)
        {
            Console.WriteLine("Recieving message:");
            Console.WriteLine(message.GetData().ToString());
            
            switch(message.GetSuperOp())
            {
                case 1:
                    ProcessPlayerMesssage(client, message);
                    break;
            }
        }

        #region Player Messages
        private void ProcessPlayerMesssage(Client client, Message message)
        {

            switch(message.GetSubOp())
            {
                case 1:
                    PlayerDirectionChange(client, message);
                    break;
            }
        }

        private void PlayerDirectionChange(Client client, Message message)
        {
            byte[] data = message.GetData();
            string name = "Player_" + Clients.IndexOf(client) + "_Movable";
            short direction = data[0];

            Movable player = (Movable)SystemManager.Get(SystemTypes.Movable, name);
            player.Direction.x = direction;
        }

        private void PlayerUsePowerUp(Message message)
        {

        }

        private void PlayerReady(Message message)
        {

        }

        #endregion

    }
}
