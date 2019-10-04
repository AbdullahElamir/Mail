using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.DTOs
{
    public class DetailsConversationDTO
    {
        public long ConversationID { get; set; }
        public string SubjectBody { get; set; }
        public string Subject { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public long? UserId { get; set; }
        public string DateConversation { get; set; }
        public string TimeConversation { get; set; }
        public string Priolti { get; set; }
        public bool? Replay { get; set; }

        public List<MessageDTO> MessageDTOs { get; set; }
        public List<AttachmentDTO> AttachmentDTOs { get; set; }
    }
}
