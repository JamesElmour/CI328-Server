using System.Collections.Generic;

namespace PIGMServer.Network
{
    /// <summary>
    /// Queue of Messages for sending or collation.
    /// </summary>
    public class MessageQueue
    {
        // List of Messages to send.
        private List<Message> Queue = new List<Message>();

        /// <summary>
        /// Add collection of Messages to queue.
        /// </summary>
        /// <param name="messages">Messages to add.</param>
        public void Add(IEnumerable<Message> messages)
        {
            Queue.AddRange(messages);
        }

        /// <summary>
        /// Get all Messages in Queue.
        /// </summary>
        /// <returns></returns>
        public List<Message> Get()
        {
            return Queue;
        }

        /// <summary>
        /// Collate all contained Messages into single Message.
        /// </summary>
        /// <returns></returns>
        public Message Collate()
        {
            // List of Data for bytes.
            List<byte> bytes = new List<byte>();

            // Encode each Message into byte list.
            foreach(Message m in Queue)
            {
                bytes.AddRange(m.Encode(false));
                bytes.Add(255); // Terminator byte, signaling end of Message.
            }

            // Create and return Message.
            Message message = new Message(0, 4, bytes.ToArray());
            return message;
        }
    }
}
