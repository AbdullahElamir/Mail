using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SMSServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MessageServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MessageServices.svc or MessageServices.svc.cs at the Solution Explorer and start debugging.
    public class MessageServices : IMessageServices
    {
        public async Task<bool> SendSMS(string sms, List<string> phones)
        {
            try
            {
                await Task.FromResult(true);
                GsmCommMain comm = new GsmCommMain("COM6", 1, 80000);
                if (!comm.IsOpen())
                        comm.Open();

                foreach(var phone in phones)
                {
                    SmsSubmitPdu[] messagePDU = SmartMessageFactory.CreateConcatTextMessage(sms, true, phone);
                    comm.SendMessages(messagePDU);
                }
              
                comm.Close();
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
