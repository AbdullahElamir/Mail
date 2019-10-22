using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Managegment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Managegment.Controllers
{
    [Produces("application/json")]
    [Route("api/SystemInformation")]
    public class SystemInformationController : Controller
    {
        private readonly MailSystemContext db;
        private Helper help;

        public SystemInformationController(MailSystemContext context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("getInforamtionMessages")]
        public async Task<ActionResult> getInforamtionMessages()
        {
            try
            {
                await Task.FromResult(true);
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 1)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                var Query = db.Participations.Where(w => w.UserId == userId
                              && w.IsDelete == false
                              && w.Archive == false &&
                              w.IsRead == false &&
                              (w.Conversation.Creator != userId ||
                              (w.Conversation.Creator == userId && w.Conversation.Messages.Any())
                          ));

                var countReplayMessageUnRead = await Query.Select(s => new  
                {
                    MessageCountNotRead = s.Conversation.Messages.
                     Where(w => w.Transactions.Any(a => a.IsRead == false && a.UserId == userId)).Count(),
                }).SumAsync(s=>s.MessageCountNotRead);

                var result = new ResultInformation
                {
                    CountReplayMessageUnRead = countReplayMessageUnRead,
                    CountMessageUnRead = Query.ToList().Count()
                };

                return Ok(new { data =result});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

    }

    internal class ResultInformation
    {
        public int CountReplayMessageUnRead { get; set; }
        public int CountMessageUnRead { get; set; }
    }
}