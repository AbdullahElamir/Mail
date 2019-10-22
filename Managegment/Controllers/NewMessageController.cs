using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Managegment.DTOs;
using Managegment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Utils;
using System.IO;
using Managegment.Services;

namespace Managegment.Controllers
{
    [Produces("application/json")]
    [Route("api/NewMessage")]
    public class NewMessageController : Controller
    {
        private readonly MailSystemContext db;
        //private readonly MessageServicesClient _messageServicesClient;
        private Helper help;
        public NewMessageController(MailSystemContext context)
        {
            this.db = context;
            help = new Helper();
        }
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                var result = await db.Users.Where(w=>w.UserId != userId).Select(s => new UsersDTO
                {
                    Email = s.Email,
                    UserName = s.FullName,
                    UserId=s.UserId
                }).ToListAsync();
                return Ok(new { AllUsers = result });

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpGet("GetAllAdTypes")]
        public async Task<ActionResult> GetAllAdTypes()
        {
            try
            {
                var result = await db.AdTypes.Where(w => w.Status != 9).Select(s => new
                {
                    AdTypeName = s.AdTypeName,
                    AdTypeId = s.AdTypeId
                }).ToListAsync();
                return Ok(new { data= result });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("NewMessage")]
        public async Task <ActionResult> NewMessage([FromBody] NewMessageDTO newMessageDTO)
        {
            try
            {
                if (newMessageDTO == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                if (newMessageDTO.Selectedusers == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");
                }

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                List<long> userRecevies = new List<long>();
                foreach (var item in newMessageDTO.Selectedusers)
                {
                    userRecevies.Add(item);
                }
                userRecevies.Add(userId);
                var dateCreate = DateTime.Now;

                var messageWithOutHTML = help.GetPlainTextFromHtml(newMessageDTO.Content);
             

                var conversationId= await saveConversations(newMessageDTO, userId, messageWithOutHTML, dateCreate);

                foreach (var item in userRecevies)
                {
                    await saveParticipations(item, conversationId,newMessageDTO.SelectedOption, dateCreate,userId);
                }

                if(newMessageDTO.Files.Length>0)
                {
                    await saveAttachment(newMessageDTO, userId, conversationId, dateCreate);
                }
                await db.SaveChangesAsync();
                
                return Ok("تم عملية الارسال التعميم بنجاح سيتم ");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private async Task<long>  saveConversations( NewMessageDTO newMessageDTO ,long userId,string LastSubject,DateTime dateTime)
        {
            Conversations conversations = new Conversations()
            {
                LastSubject = LastSubject,
                AdTypeId = newMessageDTO.Type,
                Body = newMessageDTO.Content,
                Priolti = newMessageDTO.Priority,
                TimeStamp = dateTime,
                Subject = newMessageDTO.Subject,
                IsGroup = newMessageDTO.Replay,
                Creator = userId,
                TypeSend= (short)newMessageDTO.SelectedOption
            };
            await db.Conversations.AddAsync(conversations);
            db.Entry(conversations).State = EntityState.Added;
            return conversations.ConversationId;
          
        }

        public async Task saveParticipations(long userId,long conversationId, SelectedOption selectedOption, DateTime dateTime, long currentUser)
        {
            Participations participations=null;
            bool? IsSendSms = null;
            bool? IsSendEmail = null;
            
            switch (selectedOption)
            {
                case SelectedOption.All:
                    IsSendSms = false;
                    IsSendEmail = false;
               
                    break;
                case SelectedOption.Email:
                    IsSendEmail = false;
                    
                    break;
                case SelectedOption.SMS:
                    IsSendSms = false;
                    break;
            }
            participations = new Participations()
            {
                ConversationId = conversationId,
                Archive = false,
                UserId = userId,
                CreatedOn = dateTime,
                IsDelete = false,
                IsFavorate = false,
                IsRead = false,
                IsSendEmail =(userId == currentUser) ? null : IsSendEmail,
                IsSendSms = (userId == currentUser) ? null : IsSendSms,
                ExpiryDate=  dateTime.AddMinutes(1),
                TransactionProccess= (selectedOption== SelectedOption.NONE || userId==currentUser)?(short)State.NotSelected: (short)State.proccess
            };
            await db.Participations.AddRangeAsync(participations);
        }

        // Attachment
        private async Task saveAttachment(NewMessageDTO newMessageDTO, long userId,long conversationId, DateTime dateTime)
        {
            foreach(var item in newMessageDTO.Files)
            {
                await db.Attachments.AddAsync(new Attachments()
                {
                    ConversationId= conversationId,
                    FileName = item.FileName,
                    Extension=item.Type,
                    ContentFile= Convert.FromBase64String(item.FileBase64.Substring(item.FileBase64.IndexOf(",") + 1)),
                    CreatedBy=userId,
                    CreatedOn= dateTime
                });
            }
        }

        [HttpGet("DownLoadFile")]
        public async Task<IActionResult> DownLoadFile(long attachmentId)
        {
            try
            {
                if (attachmentId <= 0)
                {
                    return StatusCode(401, "هناك مشكلة في تحميل الملف");
                }
                var result = await db.Attachments.SingleOrDefaultAsync(s => s.AttachmentId == attachmentId);
                return File(result.ContentFile, result.Extension, result.FileName);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}