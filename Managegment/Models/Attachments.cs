using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Attachments
    {
        public long AttachmentId { get; set; }
        public long? ConversationId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] ContentFile { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }

        public virtual Conversations Conversation { get; set; }
        public virtual Users CreatedByNavigation { get; set; }
    }
}
