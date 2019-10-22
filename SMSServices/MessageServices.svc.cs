using GsmComm.GsmCommunication;
using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMSServices
{
    public  class MessageServices : IMessageServices
    {
        public  MessageServices()
        {
        
        }

        public async Task<List<UserToSendEmailOrSMS>> SendMessageSMS(List<SendMessages> messages)
        {
            GsmCommMain comm  = new GsmCommMain("COM6", 115200, 60000);
            if (!comm.IsOpen())
                comm.Open();
            List<UserToSendEmailOrSMS> smsSuccess = new List<UserToSendEmailOrSMS>();
            try
            {
                await Task.FromResult(true);
                //if (!comm.IsOpen())
                //    comm.Open();
                foreach(var itemMessage in messages)
                {
                    string messageBody = "";
                    string messageFile = "";
                    var body = GetPlainTextFromHtml(itemMessage.SubjectBody);
                    string intro = string.Format(@"وردك ({0}) تحت عنوان ({1})", itemMessage.AdTypeName, itemMessage.Subject);
                    if (itemMessage.IsAttachment == true)
                    {
                        messageFile = "(يوجد لديك ملفات مرفقة)";
                    }
                    if ((body.Length + intro.Length + messageFile.Length) > 150)
                    {
                        messageBody = intro + "\n" + messageFile;
                    }
                    else
                    {
                        messageBody = intro + "\n" + body + "\n" + messageFile;
                    }

                    foreach(var sendPhone in itemMessage.UsersSender)
                    {
                            try
                            {
                            
                          
                            string txtDestinationNumbers = sendPhone.Phone;
                                SmsSubmitPdu[] messagePDU =SmartMessageFactory.CreateConcatTextMessage(messageBody, true, txtDestinationNumbers);
                                comm.SendMessages(messagePDU);
                                smsSuccess.Add(new UserToSendEmailOrSMS()
                                {
                                    ConversationID = itemMessage.ConversationID,
                                    IsSendSMS = true,
                                    UserID = sendPhone.UserID,
                                });
                            }
                        catch (Exception ex)
                            {
                                 
                            }
                    }

                }
                comm.Close();
                return smsSuccess;
            }
            catch (Exception ex)
            {
            }
            comm.Close();
            return smsSuccess;
        }

        public async Task<List<UserToSendEmailOrSMS>> SendSMS(string sms, List<UserToSendEmailOrSMS> phones)
        {
            
            GsmCommMain comm = new GsmCommMain("COM6", 115200, 2000);
            List<UserToSendEmailOrSMS> smsSuccess = new List<UserToSendEmailOrSMS>();
            try
            {
                string txtMessage = "سلام عليكم";
                await Task.FromResult(true);
                    if (!comm.IsOpen())
                            comm.Open();
                //foreach (var item in phones)
                //{
                    try
                    {
                       
                        string txtDestinationNumbers = "0922682985";

                        SmsSubmitPdu[] messagePDU = SmartMessageFactory.CreateConcatTextMessage(txtMessage, true, txtDestinationNumbers);
                        comm.SendMessages(messagePDU);
                        //smsSuccess.Add(new UserToSendEmailOrSMS()
                        //{
                        //    ConversationID = item.ConversationID,
                        //    IsSendSMS = true,
                        //    UserID = item.UserID,
                        //});
                    }
                    catch (Exception ex)
                    {

                      
                    }
                  
                  //}

                comm.Close();
                return smsSuccess;
            }
            catch (Exception ex)
            {
            }
            comm.Close();
            return smsSuccess;
        }


        private string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);
            return htmlString;
        }


    }
}
