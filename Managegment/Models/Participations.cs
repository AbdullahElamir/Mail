using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Participations
    {
        public long ConversationId { get; set; }
        public long UserId { get; set; }
        public bool? Archive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsFavorate { get; set; }

        public Conversations Conversation { get; set; }
        public Users User { get; set; }
    }
}
