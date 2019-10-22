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
        public bool? IsSendSms { get; set; }
        public bool? IsSendEmail { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public short? TransactionProccess { get; set; }

        public virtual Conversations Conversation { get; set; }
        public virtual Users User { get; set; }
    }
}
