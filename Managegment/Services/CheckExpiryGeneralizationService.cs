using Managegment.DTOs;
using Managegment.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Managegment.Services
{
    public class CheckExpiryGeneralizationService
    {

        private readonly MailSystemContext db;
        public CheckExpiryGeneralizationService()
        {
              db = new MailSystemContext();
        }

        public async Task CheckExpiryDateGeneralization()
        {
            try
            {
                var dateFrom = DateTime.Now;
                DbFunctions dfunc = null;

                var query = db.Participations.Where(w => w.UserId != w.Conversation.Creator &&
                ((SqlServerDbFunctionsExtensions.DateDiffMinute(dfunc, Convert.ToDateTime(dateFrom), Convert.ToDateTime(w.ExpiryDate.Value)) <= 0) &&
                (w.IsSendEmail == false || w.IsSendSms == false)) && w.TransactionProccess==(short)State.proccess );
                foreach (var item in query)
                {
                    item.TransactionProccess =(short) State.failed ;
                    db.Entry(item).State = EntityState.Modified;
                }
                await db.SaveChangesAsync();

            }
            catch (Exception)
            {

            }
        }

    }
}
