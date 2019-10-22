using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMSServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMessageServices" in both code and config file together.
    [ServiceContract]
    public interface IMessageServices
    {
        [OperationContract]
        Task<List<UserToSendEmailOrSMS>> SendSMS(string sms,List<UserToSendEmailOrSMS> phone);
        [OperationContract]
        Task<List<UserToSendEmailOrSMS>> SendMessageSMS(List<SendMessages> messages);
       
    }
    [DataContract]
    public class UserToSendEmailOrSMS
    {
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long ConversationID { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public bool? IsSendSMS { get; set; }
    }

    [DataContract]
    public class SendMessages
    {
        [DataMember]
        public long ConversationID { get; set; }
        [DataMember]
        public string SubjectBody { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public long? UserSenderID { get; set; }
        [DataMember]
        public string DateConversation { get; set; }
        [DataMember]
        public string AdTypeName { get; set; }
        [DataMember]
        public bool IsAttachment { get; set; }
        [DataMember]
        public List<UserToSendEmailOrSMS> UsersSender { get; set; }

    }
}
