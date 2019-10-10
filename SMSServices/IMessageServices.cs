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
        Task<bool> SendSMS(string sms,List<string> phone);
    }
}
