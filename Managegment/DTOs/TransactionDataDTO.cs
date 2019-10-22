using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.DTOs
{
    public class TransactionDataDTO
    {

        public long ConversationID { get; set; }
        public short? TypeSend { get; set; }
        public long? UserIdCreator { get; set; }
        public string SubjectBody { get; set; }
        public string ContactBodyWithSubject { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
        public string DateConversation { get; set; }
        public State ConverationState { get; set; }
        public List<TransactionUserDataDTO> UsersSender { get; set; }

    }

    public class TransactionUserDataDTO
    {
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ExpiryDate { get; set; }
        public State IsSendEmail { get; set; }
        public State IsSendSMS { get; set; }
    }

    public enum State
    {
        success,
        failed,
        proccess,
        NotSelected,
        NONE
    }
}
