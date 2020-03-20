using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIGMServer.Network
{
    public class MessageQueue
    {
        private List<Message> Queue = new List<Message>();

        public MessageQueue()
        {

        }

        public void Add(Message message)
        {
            Queue.Add(message);
        }

        public void Add(IEnumerable<Message> messages)
        {
            Queue.AddRange(messages);
        }

        public List<Message> Get()
        {
            return Queue;
        }

        public void Clear()
        {
            Queue.Clear();
        }
    }
}
