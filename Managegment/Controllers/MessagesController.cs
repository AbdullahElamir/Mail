using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Managegment.DTOs;
using Managegment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Managegment.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private readonly MailSystemContext db;
        private Helper help;
      
        public MessagesController(MailSystemContext context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("GetAllInbox")]
        public async Task<ActionResult> GetAllInbox(int page,int pageSize)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return NotFound();
                }
                
                var result = await db.Participations.Where(w => w.UserId == userId
                     && w.IsDelete == false
                     && w.Archive == false &&
                     (w.Conversation.Creator != userId ||
                     (w.Conversation.Creator == userId && w.Conversation.Messages.Any())
                     )).Select(s => new ConversationDTO
                     {
                         ConversationID = s.ConversationId,
                         IsRead = s.IsRead,
                         Subject = s.Conversation.Subject,
                         LastSubjectBody = s.Conversation.Subject+" - " +  s.Conversation.LastSubject,
                         DateConversation = s.Conversation.TimeStamp.Value.ToString("hh:mm dd MMM"),
                         TimeConversation = s.Conversation.TimeStamp.Value.ToLongTimeString(),
                         SubjectBody = s.Conversation.Body,
                         UserId = s.Conversation.Creator.ToString(),
                         UserName = s.Conversation.Participations.
                         SingleOrDefault(w => w.User.UserId == s.Conversation.Creator).User.FullName,
                         IsFavorate = s.IsFavorate,
                         Priolti = s.Conversation.Priolti,
                         MessageCountNotRead = s.Conversation.Messages. 
                         Where(w => w.Transactions.Any(a => a.IsRead == false && a.UserId == userId)).Count(),
                         IsAttachment=s.Conversation.Attachments.Any(),
                         IsArchive=s.Archive
                     }).Skip((1 - 1) * 20).Take(20).ToListAsync();
                    return Ok(new { Inbox= result });
                
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }
        [HttpGet("EnabelDisplayFavorate")]
        public async Task<ActionResult> IsFavorate(bool isFavorate,string conversationId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId<=1)
                {
                    return NotFound();
                }
                
                var result =await db.Participations.SingleOrDefaultAsync(s => s.ConversationId == conversationId &&
                  s.UserId == userId);
                result.IsFavorate = isFavorate;
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new {state=true});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("Archive")]
        public async Task<ActionResult> IsArchive(bool isArchive,string conversationId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return NotFound();
                }
                var result = await db.Participations.SingleOrDefaultAsync(s => s.ConversationId == conversationId &&
                   s.UserId == userId);
                result.Archive = isArchive;
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new { state = true });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("DeleteConversation")]
        public async Task<ActionResult> DeleteConversation(bool isDelete, string conversationId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return NotFound();
                }
                 
                var result = await db.Participations.SingleOrDefaultAsync(s => s.ConversationId == conversationId &&
                   s.UserId == userId);
                result.IsDelete = isDelete;
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return Ok(new { state = true });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet("ReadUnReadInbox")]
        public async Task<ActionResult> ReadUnReadInbox(bool isReadUnRead, string conversationId)
        {
            try
            {
                 var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return NotFound();
                }

                var isReadUnread = await Read_UnRead(userId, conversationId, isReadUnRead);
                if(isReadUnread)
                    await  db.SaveChangesAsync();

                return Ok(new { state = true });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [Route("getContentConversation")]
        public async Task<ActionResult> getContentConversation(string conversationId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return NotFound();
                }

                var isReadUnread =await Read_UnRead(userId, conversationId, true);

                var result = await db.Conversations.
                Select(conversation => new DetailsConversationDTO()
                {
                    ConversationID = conversation.ConversationId,
                    DateConversation = conversation.TimeStamp.Value.ToString("dd/mm/yyyy"),
                    TimeConversation= conversation.TimeStamp.Value.ToString("ddd, dd MMM yyyy hh:mm tt"),
                    Subject = conversation.Subject,
                    Priolti = conversation.Priolti,
                    SubjectBody = conversation.Body,
                    UserName = conversation.Participations.
                    SingleOrDefault(user => user.User.UserId == conversation.Creator).User.FullName,
                    UserId = conversation.Creator,
                    MessageDTOs = conversation.Messages.Select(message => new MessageDTO()
                    {
                        DateTime = message.DateTime.Value.ToString("ddd, dd MMM yyyy hh:mm tt"),
                        MessageID = message.MessageId,
                        Subject = message.Subject,
                        UserName = message.Author.FullName,
                    }).ToList(),
                    AttachmentDTOs = conversation.Attachments.Select(attachment => new AttachmentDTO()
                    {
                        AttachmentId = attachment.AttachmentId,
                        ContentFile = attachment.ContentFile,
                        Extension = attachment.Extension,
                        FileName = attachment.FileName,
                        FileSize = attachment.FileSize,
                        Hash = attachment.Hash
                    }).ToList(),
                }).SingleOrDefaultAsync(s => s.ConversationID == conversationId);
                if(result !=null && isReadUnread)
                {
                    await db.SaveChangesAsync();
                }
                return Ok(new {data=result});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetAllInboxSender")]
        public async Task<ActionResult> GetAllInboxSender(int page, int pageSize)
        {
            try
            {

                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return NotFound();
                }
                var result = await db.Participations.Where(w => w.UserId == userId
                    && w.IsDelete == false
                    && w.Archive == false &&
                    w.Conversation.Creator == userId
                    ).Select(s => new ConversationDTO
                    {
                        ConversationID = s.ConversationId,
                        IsRead = s.IsRead,
                        Subject = s.Conversation.Subject,
                        LastSubjectBody = s.Conversation.Subject + " - " + s.Conversation.LastSubject,
                        DateConversation = s.Conversation.TimeStamp.Value.ToString("hh:mm dd MMM"),
                        TimeConversation = s.Conversation.TimeStamp.Value.ToLongTimeString(),
                        SubjectBody = s.Conversation.Body,
                        UserId = s.Conversation.Creator.ToString(),
                        UserName = s.Conversation.Participations.
                        SingleOrDefault(w => w.User.UserId == s.Conversation.Creator).User.FullName,
                        IsFavorate = s.IsFavorate,
                        Priolti = s.Conversation.Priolti,
                        MessageCountNotRead = s.Conversation.Messages.
                        Where(w => w.Transactions.Any(a => a.IsRead == false && a.UserId == userId)).Count(),
                        IsAttachment = s.Conversation.Attachments.Any(),
                        IsArchive = s.Archive
                    }).Skip((1 - 1) * 20).Take(20).ToListAsync();
                return Ok(new { sent =result});
                 
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        private async Task<bool> Read_UnRead(long userId,string conversationId,bool isReadUnRead )
        {
            var updateIsReadMessages = await db.Transactions.Where(w => w.UserId == userId
               && w.Message.ConversationId == conversationId && w.IsRead == false).ToListAsync();
            foreach (var item in updateIsReadMessages)
            {
                item.IsRead = true;
                db.Entry(item).State = EntityState.Modified;
            }
            var result = await db.Participations.SingleOrDefaultAsync(s => s.ConversationId == conversationId &&
               s.UserId == userId);
            result.IsRead = isReadUnRead;
            db.Entry(result).State = EntityState.Modified;
            return true;
        }
         
    }
}