using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Transactions
    {
        public long TransactionId { get; set; }
        public long? MessageId { get; set; }
        public long? UserId { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? TimeStamp { get; set; }

        public Messages Message { get; set; }
        public Users User { get; set; }
    }
}
