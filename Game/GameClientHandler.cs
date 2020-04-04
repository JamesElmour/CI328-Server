using PIGMServer.Game.Components;
using PIGMServer.Game.Systems;
using PIGMServer.Game.Worlds;
using PIGMServer.Network;
using System;
using System.Collections.Generic;

namespace PIGMServer.Game
{
    public abstract class GameClientHandler : ClientOwner
    {

        protected List<Client> Clients;


        public GameClientHandler()
        {
            Clients = new List<Client>(AcceptLimit);
        }

        public override void Give(Client client)
        {
            Clients.Add(client);
            AcceptedClients++;
        }

        public override void Remove(Client client)
        {
            Clients.Remove(client);
        }

        public override void HandleMessage(Client client, Message message)
        {            
            switch(message.GetSuperOp())
            {
                case 1:
                    ProcessPlayerMesssage(client, message);
                    break;
            }
        }

        public Client Get(int index)
        {
            return Clients[index];
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
            string name = "Player";
            short direction = data[0];

            Player player = (Player) SystemManager.Get<PlayerSystem>(name);
            player.Direction = direction;
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
