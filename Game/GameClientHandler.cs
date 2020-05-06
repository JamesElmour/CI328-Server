using PIGMServer.Network;
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
            switch (message.GetSuperOp())
            {
                case 1:
                    ProcessPlayerMesssage(client, message);
                    break;
                case 4:
                    ActivatePowerUp(client, message);
                    break;
            }
        }

        public Client Get(int index)
        {
            return Clients[index];
        }

        public bool Exists(Client client)
        {
            return Clients.Contains(client);
        }

        #region Player Messages
        private void ProcessPlayerMesssage(Client client, Message message)
        {

            switch (message.GetSubOp())
            {
                case 1:
                    PlayerPositionChange(client, message);
                    break;
            }
        }

        protected abstract void PlayerPositionChange(Client client, Message message);

        protected abstract void ActivatePowerUp(Client client, Message message);
        #endregion

    }
}
