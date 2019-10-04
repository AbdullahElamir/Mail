using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Messages
    {
        public Messages()
        {
            Transactions = new HashSet<Transactions>();
        }

        public long MessageId { get; set; }
        public long? ConversationId { get; set; }
        public long? AuthorId { get; set; }
        public DateTime? DateTime { get; set; }
        public string Subject { get; set; }

        public Users Author { get; set; }
        public Conversations Conversation { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
    }
}
