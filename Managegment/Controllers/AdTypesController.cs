using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Managegment.Controllers;
using Managegment.Models;
using Management.Controllers;

using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Controllers
{
    [Produces("application/json")]
    [Route("Api/Admin/AdTypes")]
    public class AdTypesController : Controller
    {
        private readonly MailSystemContext db;
        private Helper help;
        public AdTypesController(MailSystemContext context)
        {
            this.db = context;
            help = new Helper();
        }

        [HttpGet("Get")]
        public IActionResult Get(int pageNo, int pageSize)
        {
            try
            {

                IQueryable<AdTypes> AdTypesQuery;

                AdTypesQuery = from p in db.AdTypes
          select p;

                var AdTypesCount = (from p in AdTypesQuery
                                     select p).Count();

                var AdTypesList = (from p in AdTypesQuery
                                    
                                    select new AdTypes
                                    {
                                        AdTypeName = p.AdTypeName,

                                        AdTypeId = p.AdTypeId

                                   

                                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { AdTypes = AdTypesList, count = AdTypesCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Add")]
        public IActionResult AddAdTypes([FromBody] AdTypes AddAdType)
        {
            try
            {
                if (AddAdType == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");

                }

                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {

                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");

                }

                AddAdType.CreatedBy = userId;
                AddAdType.CreatedOn = DateTime.Now;
                AddAdType.Status = 1;
                db.AdTypes.Add(AddAdType);
                db.SaveChanges();
                return Ok("لقد قمت بتسـجيل نوع الرساله بنــجاح");

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("Edit")]
        public IActionResult EditAdTypes([FromBody] AdTypes AddAdType)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var AdTypes = (from p in db.AdTypes
                                where p.AdTypeId == AddAdType.AdTypeId
                             
                                select p).SingleOrDefault();

                if (AdTypes == null)
                {
                    return BadRequest("حذث خطأ في ارسال البيانات الرجاء إعادة الادخال");

                }

                AdTypes.AdTypeName = AddAdType.AdTypeName;
             
                db.SaveChanges();
                return Ok("تم تعديل بيانات نوع الرسالة بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}