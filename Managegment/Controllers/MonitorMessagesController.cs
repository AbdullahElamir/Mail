using Managegment.DTOs;
using Managegment.Models;
using Managegment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/MonitorMessages")]
    public class MonitorMessagesController : Controller
    {
        private readonly MailSystemContext db;
        private Helper help;
        public MonitorMessagesController(MailSystemContext context)
        {
            this.db = context;
            help = new Helper();
        }
        [HttpPost("GetTransactions")]
        public async Task<ActionResult> GetTransactions([FromBody] FilterMessages filterMessages)
        {
            

            try
            {

                var userId = help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                if (filterMessages==null)
                {
                    return StatusCode(401, "هناك مشكلة في الادخال");
                }

                if (!filterMessages.FromDate.HasValue && !filterMessages.ToDate.HasValue)
                {
                    filterMessages.FromDate = Convert.ToDateTime("2000-01-01 00:00:00.000");
                    filterMessages.ToDate = DateTime.Now;
                }

                if(!filterMessages.isAllShowMessagess.HasValue )
                {
                    filterMessages.isAllShowMessagess = false;
                }

                if(!filterMessages.SearchWithUser.HasValue)
                {
                    filterMessages.SearchWithUser = 0;
                }

                var dateNowFilter = DateTime.Now;
                DbFunctions dfunc = null;

              


                var query = db.Conversations.Where(w =>
                  (
                    (filterMessages.isAllShowMessagess.Value ? db.Users.Any(a => a.UserId == userId && a.UserType == 1) : w.Creator == userId) &&
                      (filterMessages.SearchWithUser.Value > 0 ? w.Creator == filterMessages.SearchWithUser.Value : true)
                  )
                  );

                var successConversation = query.Where(w => w.Participations.Where(s => s.UserId != w.Creator).All(a => a.TransactionProccess == (short)State.success));

                var failedConversation = query.Where(w => w.Participations.Where(s => s.UserId != w.Creator).Any(a => a.TransactionProccess == (short)State.failed));

                var processConversation = query.Where(w => w.Participations.Any(a => a.UserId != w.Creator &&
                (a.TransactionProccess == (short)State.proccess && a.TransactionProccess != (short)State.failed)));
                var SendInWebOnly = query.Where(w => w.Participations.Where(s => s.UserId != w.Creator).All(a => a.TransactionProccess == (short)State.NotSelected));

                var filterQuery = query.
                       Where(w => (SqlServerDbFunctionsExtensions.DateDiffDay(dfunc, Convert.ToDateTime(filterMessages.FromDate), Convert.ToDateTime(w.TimeStamp.Value)) >= 0 &&
                       SqlServerDbFunctionsExtensions.DateDiffDay(dfunc, Convert.ToDateTime(filterMessages.ToDate), Convert.ToDateTime(w.TimeStamp.Value)) <= 0) &&
                       (
                       (filterMessages.StateTransaction == State.success ? w.Participations.Where(s => s.UserId != w.Creator).All(a => a.TransactionProccess == (short)State.success) : true) &&
                       (filterMessages.StateTransaction == State.failed ? w.Participations.Where(s => s.UserId != w.Creator).Any(a => a.TransactionProccess == (short)State.failed) : true) &&
                       (filterMessages.StateTransaction == State.proccess ? w.Participations.Any(a => a.UserId != w.Creator &&
                       (a.TransactionProccess == (short)State.proccess && a.TransactionProccess != (short)State.failed)) : true) &&
                       (filterMessages.StateTransaction == State.NotSelected ? w.Participations.Where(s => s.UserId != w.Creator).All(a => a.TransactionProccess == (short)State.NotSelected) : true)
                       )
                       );

                var result = await filterQuery.Select(s => new TransactionDataDTO()
                {
                    ConversationID = s.ConversationId,
                    Subject = s.Subject,
                    SubjectBody = s.Body,
                    ContactBodyWithSubject= (s.Subject + " - " + s.LastSubject).Substring(0, 80) + "....",
                    DateConversation = s.TimeStamp.Value.ToString(),
                    UserName = s.CreatorNavigation.FullName,
                    UserIdCreator=s.Creator,
                    TypeSend=s.TypeSend,
                    ConverationState =
                      (
                          SendInWebOnly.Any(a => a.ConversationId == s.ConversationId) ? State.NotSelected :
                          successConversation.Any(a => a.ConversationId == s.ConversationId) ? State.success :
                          processConversation.Any(a => a.ConversationId == s.ConversationId) ? State.proccess :
                          State.failed
                      ),
                    UsersSender = s.Participations.Where(W => W.UserId != s.Creator).Select(u => new TransactionUserDataDTO()
                    {

                        IsSendSMS =
                            (
                             (s.TypeSend == 3 || s.TypeSend == 1) ? State.NotSelected :
                             (s.TypeSend == 2 || s.TypeSend == 0) && u.IsSendSms == true ? State.success :
                             (SqlServerDbFunctionsExtensions.DateDiffMinute(dfunc, Convert.ToDateTime(dateNowFilter), Convert.ToDateTime(u.ExpiryDate.Value)) >= 0 &&
                             u.TransactionProccess == (short)State.proccess) ? State.proccess :
                             State.failed
                            ),

                        IsSendEmail =
                            (
                             (s.TypeSend == 3 || s.TypeSend == 2) ? State.NotSelected :
                             (s.TypeSend == 1 || s.TypeSend == 0) && u.IsSendEmail == true ? State.success :
                             (SqlServerDbFunctionsExtensions.DateDiffMinute(dfunc, Convert.ToDateTime(dateNowFilter), Convert.ToDateTime(u.ExpiryDate.Value)) >= 0 &&
                             u.TransactionProccess==(short)State.proccess) ? State.proccess :
                             State.failed
                            ),

                        Email = u.User.Email,
                        Phone = u.User.Phone,
                        UserID = u.UserId,
                        UserName = u.User.FullName,
                        ExpiryDate =u.TransactionProccess==(short)State.NotSelected?"لايوجد صلاحية ": u.ExpiryDate.Value.ToString()
                    }).ToList()

                }).OrderByDescending(o => o.ConversationID).Skip((filterMessages.Page - 1) * filterMessages.PageSize).Take(filterMessages.PageSize).ToListAsync();


                var resultSend = new
                {
                    FailedConversation = failedConversation.Count(),
                    SuccessConversation = successConversation.Count(),
                    ProcessConversation = processConversation.Count(),
                    SendInWebOnly = SendInWebOnly.Count(),
                 
                    result
                };
                return Ok(new { data = resultSend, CountPage = filterQuery.Count()});



            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);

            }

        }


        [HttpGet("getAllUsersByBranch")]
        public async Task<ActionResult> getAllUsersByBranch(long branchId)
        {
            try
            {
                var userId = help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var result =await db.Users.Where(w => w.BranchId == branchId && w.Status != 9 && w.UserId != userId).Select(s => new
                {
                    UserName=s.FullName,
                    UserId=s.UserId
                }).ToListAsync();

                return Ok(new { users = result });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("reaplaySendMessagesToSMS")]
        public async Task<ActionResult> ReaplaySendMessagesToSMS(long userId,long conversationId)
        {
            try
            {
                var CurrentUserId = help.GetCurrentUser(HttpContext);
                if (CurrentUserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                bool issuccess = false;
                ServiceSend serviceSend = new ServiceSend();

                var result = await getReplayUserSMSOrEmail(userId, conversationId);

                var sendSMS = await serviceSend.SendSMS(result);
                if(sendSMS)
                {
                    issuccess = true;
                }
             

                return Ok(new { State = issuccess });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);

            }
        } 
        
        [HttpGet("replaySendEmail")]
        public async Task<ActionResult> replaySendEmail(long userId,long conversationId)
        {
            try
            {

                var CurrentUserId = help.GetCurrentUser(HttpContext);
                if (CurrentUserId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                bool issuccess = false;
                ServiceSend serviceSend = new ServiceSend();



                var result =await getReplayUserSMSOrEmail(userId,conversationId);

                var sendEmail = await serviceSend.SendEmailToUsers(result);
                if(sendEmail)
                {
                    issuccess = true;
                }
             

                return Ok(new { State = issuccess });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);

            }
        }

        private async Task<List<SendMessages>> getReplayUserSMSOrEmail(long userId, long conversationId)
        {
            var query = db.Conversations.Where(w => w.Participations.Any(a => a.UserId == userId && a.ConversationId == conversationId));

            var result = await query.Select(s => new SendMessages()
            {
                ConversationID = s.ConversationId,
                Subject = s.Subject,
                SubjectBody = s.Body,
                IsAttachment = s.Attachments.Any(),
                UserSenderID = s.Creator,
                UserName = s.CreatorNavigation.FullName,
                Priolti = s.Priolti,
                DateConversation = s.TimeStamp.Value.ToString("HH:mm dd MMM"),
                TimeConversation = s.TimeStamp.Value.ToLongTimeString(),
                AdTypeName = s.AdType.AdTypeName,
                TypeSend = s.TypeSend,
                BranchName = s.CreatorNavigation.Branch.Name,
                UsersSender = s.Participations.Where(a => a.UserId == userId && a.ConversationId == conversationId).
                Select(user => new UserToSendEmailOrSMS()
                {
                    Email = user.User.Email,
                    Phone = user.User.Phone,
                    UserID = user.User.UserId,
                    UserName = user.User.FullName,
                    IsSendEmail = user.IsSendEmail,
                    IsSendSMS = user.IsSendSms,
                    ConversationID = user.ConversationId,
                    TransactionProccess = user.TransactionProccess
                }).ToList()
            }).ToListAsync();
            return result;
        }


    }


    



    public class FilterMessages
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 20;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public State StateTransaction { get; set; } = State.NONE;
        public bool? isAllShowMessagess { get; set; } = false;
        public long? SearchWithUser { get; set; } = 0;

    }

    public class Test
    {
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
