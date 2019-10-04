using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.DTOs
{
    public class ConversationDTO
    {
        public long ConversationID { get; set; }
        public string SubjectBody { get; set; }
        public string LastSubjectBody { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string DateConversation { get; set; }
        public string TimeConversation { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsFavorate { get; set; }
        public int MessageCountNotRead { get; set; }
        public string Priolti { get; set; }
        public bool? IsAttachment { get; set; }
        public bool? IsArchive { get; set; }
    }
}
