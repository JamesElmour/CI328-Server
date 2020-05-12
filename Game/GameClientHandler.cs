using PIGMServer.Network;
using System.Collections.Generic;

namespace PIGMServer.Game
{
    /// <summary>
    /// Interface designed to handle game logic and interact with clients.
    /// </summary>
    public abstract class GameClientHandler : ClientOwner
    {
        protected List<Client> Clients; // List of connected Clients.

        /// <summary>
        /// Create the Game Client Handler.
        /// </summary>
        public GameClientHandler()
        {
            // Set up Clients List with default accept limit.
            Clients = new List<Client>(AcceptLimit);
        }

        /// <summary>
        /// Give the Client the Game Client Handler.
        /// </summary>
        /// <param name="client">TCP Client to give.</param>
        public override void Give(Client client)
        {
            // Add Client.
            Clients.Add(client);
            AcceptedClients++;
        }

        /// <summary>
        /// Remove given Client from the Game Client Handler.
        /// </summary>
        /// <param name="client">Client to remove.</param>
        public override void Remove(Client client)
        {
            Clients.Remove(client);
        }

        /// <summary>
        /// Handle Messages sent by the connected players.
        /// </summary>
        /// <param name="client">Client the message is from.</param>
        /// <param name="message">Message containing data.</param>
        public override void HandleMessage(Client client, Message message)
        {
            switch (message.GetSuperOp())
            {
                case (int) SuperOps.Player: // If Message's SuperOp is regarding the Player, process it.
                    ProcessPlayerMesssage(client, message);
                    break;
                case (int) SuperOps.PowerUp: // If Message is regarding PowerUps, process it.
                    ActivatePowerUp(client, message);
                    break;
            }
        }

        #region Player Messages

        /// <summary>
        /// Process the given Message regarding the Player.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void ProcessPlayerMesssage(Client client, Message message)
        {
            // If the SubOp dictates moving the Player, change Player's position accordingly.
            switch (message.GetSubOp())
            {
                case (int) PlayerOps.PositionUpdate:
                    PlayerPositionChange(client, message);
                    break;
            }
        }

        protected abstract void PlayerPositionChange(Client client, Message message);

        protected abstract void ActivatePowerUp(Client client, Message message);
        #endregion

    }
}
