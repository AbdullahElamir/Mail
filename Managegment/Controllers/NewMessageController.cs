using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Managegment.DTOs;
using Managegment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Managegment.Controllers
{
    [Produces("application/json")]
    [Route("api/NewMessage")]
    public class NewMessageController : Controller
    {
        private readonly MailSystemContext db;
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
        public async Task <IActionResult> NewMessage([FromBody] NewMessageDTO newMessageDTO)
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

                List<long> userRecevies = new List<long>();
                foreach (var item in newMessageDTO.Selectedusers)
                {
                    userRecevies.Add(item);
                }
                userRecevies.Add(userId);
                var messageWithOutHTML = help.GetPlainTextFromHtml(newMessageDTO.Content);
                if (userId <= 1)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var conversationId =await saveConversations(newMessageDTO,userId, messageWithOutHTML);
               
                foreach (var item in userRecevies)
                {
                   await saveParticipations(item, conversationId);
                }

                //send Text in SMS

                //if(messageWithOutHTML.Length<150)
                //{
                //    //send To Sms
                //}

                //Send Files In Email Gmail

                await db.SaveChangesAsync();
                return Ok("تم عملية الارسال التعميم بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private async Task<string> saveConversations( NewMessageDTO newMessageDTO ,long userId,string LastSubject)
        {
            var conversationId = Guid.NewGuid().ToString();
            await db.Conversations.AddAsync(new Conversations()
            {
                ConversationId = conversationId,
                LastSubject = LastSubject,
                AdTypeId= newMessageDTO.Type,
                Body=newMessageDTO.Content,
                Priolti=newMessageDTO.Priority,
                TimeStamp = DateTime.Now,
                Subject = newMessageDTO.Subject,
                IsGroup = newMessageDTO.Replay,
                Creator = userId
            });
            return conversationId;
        }

        public async Task saveParticipations(long userId, string conversationId)
        {

            await db.Participations.AddRangeAsync
                (
                  new Participations()
                  {
                      Archive = false,
                      ConversationId = conversationId,
                      UserId = userId,
                      CreatedOn=DateTime.Now,
                      IsDelete=false,
                      IsFavorate=false,
                      IsRead=false,
                  }
                );
        }

        // Attachment
        
    }
}