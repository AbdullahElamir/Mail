using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Attachments
    {
        public string AttachmentId { get; set; }
        public string ConversationId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public byte[] ContentFile { get; set; }
        public double? FileSize { get; set; }
        public string Hash { get; set; }

        public Conversations Conversation { get; set; }
    }
}
