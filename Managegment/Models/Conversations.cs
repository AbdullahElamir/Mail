using System;
using System.Collections.Generic;

namespace Managegment.Models
{
    public partial class Conversations
    {
        public Conversations()
        {
            Attachments = new HashSet<Attachments>();
            Messages = new HashSet<Messages>();
            Participations = new HashSet<Participations>();
        }

        public long ConversationId { get; set; }
        public string LastSubject { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Subject { get; set; }
        public bool? IsGroup { get; set; }
        public long? Creator { get; set; }
        public string Body { get; set; }
        public string Priolti { get; set; }
        public long? AdTypeId { get; set; }
        public short? TypeSend { get; set; }

        public virtual AdTypes AdType { get; set; }
        public virtual Users CreatorNavigation { get; set; }
        public virtual ICollection<Attachments> Attachments { get; set; }
        public virtual ICollection<Messages> Messages { get; set; }
        public virtual ICollection<Participations> Participations { get; set; }
    }
}
