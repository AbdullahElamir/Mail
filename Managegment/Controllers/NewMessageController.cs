using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Managegment.DTOs;
using Managegment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Utils;
using System.IO;
using SMSService;

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

                var conversationId= await saveConversations(newMessageDTO, userId, messageWithOutHTML);

                foreach (var item in userRecevies)
                {
                    await saveParticipations(item, conversationId);
                }

                if(newMessageDTO.Files.Length>0)
                {
                    await saveAttachment(newMessageDTO, userId, conversationId);
                }
                await db.SaveChangesAsync();
                var AdTypeName = db.AdTypes.SingleOrDefault(s => s.AdTypeId == newMessageDTO.Type).AdTypeName;
                switch (newMessageDTO.SelectedOption)
                {
                    case SelectedOption.All:
                        // EmIal and SMS
                        await SendSMS(newMessageDTO, AdTypeName);
                        await SendEmailToUsers(newMessageDTO,userId, AdTypeName);
                        break;
                    case SelectedOption.Email:
                        //Email
                        await SendEmailToUsers(newMessageDTO,userId, AdTypeName);
                        break;
                    case SelectedOption.SMS:
                        //SMS
                        await SendSMS(newMessageDTO, AdTypeName);
                        break;
                }
                return Ok("تم عملية الارسال التعميم بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        private async Task<long>  saveConversations( NewMessageDTO newMessageDTO ,long userId,string LastSubject)
        {
            Conversations conversations = new Conversations()
            {
                LastSubject = LastSubject,
                AdTypeId = newMessageDTO.Type,
                Body = newMessageDTO.Content,
                Priolti = newMessageDTO.Priority,
                TimeStamp = DateTime.Now,
                Subject = newMessageDTO.Subject,
                IsGroup = newMessageDTO.Replay,
                Creator = userId
            };
            await db.Conversations.AddAsync(conversations);
            db.Entry(conversations).State = EntityState.Added;
            return conversations.ConversationId;
          
        }

        public async Task saveParticipations(long userId,long conversationId)
        {
            await db.Participations.AddRangeAsync
                (
                  new Participations()
                  {
                      ConversationId= conversationId,
                      Archive = false,
                      UserId = userId,
                      CreatedOn=DateTime.Now,
                      IsDelete=false,
                      IsFavorate=false,
                      IsRead=false,
                  }
                );
        }
        // Attachment
        private async Task saveAttachment(NewMessageDTO newMessageDTO, long userId,long conversationId)
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
                    CreatedOn=DateTime.Now
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




        private async Task<bool> SendEmailToUsers(NewMessageDTO newMessageDTO,long userId,string AdTypeName)
        {
            try
            {
                List<MailboxAddress> userEmails = new List<MailboxAddress>();
                var userData = await GetUsers();
                foreach (var item in newMessageDTO.Selectedusers)
                {
                    var userSendEmail = userData.SingleOrDefault(w => w.UserId == item);
                    userEmails.Add(new MailboxAddress(userSendEmail.UserName, userSendEmail.EmailUser));
                }
                var userSender = userData.SingleOrDefault(s=>s.UserId== userId);
                await SendEmail(userEmails, newMessageDTO, AdTypeName, userSender);
                return true;
            }
            catch (Exception)
            {

            }
            return false;
        }
        private async Task SendEmail(List<MailboxAddress> userEmail, NewMessageDTO newMessageDTO,string AdTypeName, UserSendEmailAndSMS userSender)
        {
            try
            {

                string fillAttach = "";
                if (newMessageDTO.Files.Length > 0)
                {
                    fillAttach = "لقد تم ارفاق ملفات مرفقة";
                }

                var messageEmail = new MimeMessage();
                messageEmail.From.Add(new MailboxAddress("مصلحة الاحوال المدنية", "mailkittest1@gmail.com"));
                messageEmail.Subject = newMessageDTO.Subject;
                var bodyBuilder = new BodyBuilder();
               
                    messageEmail.To.AddRange(userEmail);
                    //var image1 = bodyBuilder.LinkedResources.Add(@"./Img/87001570133293444.png");
                   // image1.ContentId = MimeUtils.GenerateMessageId();
                    bodyBuilder.HtmlBody = string.Format(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html>
<head>
    <meta charset=""UTF-8"">
    <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
    <meta name=""x-apple-disable-message-reformatting"">
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
    <meta content=""telephone=no"" name=""format-detection"">
    <title></title>
    <!--[if (mso 16)]>
    <style type=""text/css"">
    a {{text-decoration: none;}}
    </style>
    <![endif]-->
    <!--[if gte mso 9]><style>sup {{ font-size: 100% !important; }}</style><![endif]-->
    <!--[if !mso]><!-- -->
    <link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,400i,700,700i"" rel=""stylesheet"">
    <!--<![endif]-->
    <style type=""""text/css"""">
    /* CONFIG STYLES Please do not delete and edit CSS styles below */
/* IMPORTANT THIS STYLES MUST BE ON FINAL EMAIL */
        #outlook a {{
            padding: 0;
        }}

        .ExternalClass {{
            width: 100%;
        }}

        .ExternalClass,
        .ExternalClass p,
        .ExternalClass span,
        .ExternalClass font,
        .ExternalClass td,
        .ExternalClass div {{
            line-height: 100%;
        }}

        .es-button {{
            mso-style-priority: 100 !important;
            text-decoration: none !important;
        }}

        a[x-apple-data-detectors] {{
            color: inherit !important;
            text-decoration: none !important;
            font-size: inherit !important;
            font-family: inherit !important;
            font-weight: inherit !important;
            line-height: inherit !important;
        }}

        .es-desk-hidden {{
            display: none;
            float: left;
            overflow: hidden;
            width: 0;
            max-height: 0;
            line-height: 0;
            mso-hide: all;
        }}

        /*
        END OF IMPORTANT
        */
        html,
        body {{
            width: 100%;
            font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
        }}

        table {{
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
            border-collapse: collapse;
            border-spacing: 0px;
        }}

        table td,
        html,
        body,
        .es-wrapper {{
            padding: 0;
            Margin: 0;
        }}

        .es-content,
        .es-header,
        .es-footer {{
            table-layout: fixed !important;
            width: 100%;
        }}

        img {{
            display: block;
            border: 0;
            outline: none;
            text-decoration: none;
            -ms-interpolation-mode: bicubic;
        }}

        table tr {{
            border-collapse: collapse;
        }}

        p,
        hr {{
            Margin: 0;
        }}

        h1,
        h2,
        h3,
        h4,
        h5 {{
            Margin: 0;
            line-height: 120%;
            mso-line-height-rule: exactly;
            font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;
        }}

        p,
        ul li,
        ol li,
        a {{
            -webkit-text-size-adjust: none;
            -ms-text-size-adjust: none;
            mso-line-height-rule: exactly;
        }}

        .es-left {{
            float: left;
        }}

        .es-right {{
            float: right;
        }}

        .es-p5 {{
            padding: 5px;
        }}

        .es-p5t {{
            padding-top: 5px;
        }}

        .es-p5b {{
            padding-bottom: 5px;
        }}

        .es-p5l {{
            padding-left: 5px;
        }}

        .es-p5r {{
            padding-right: 5px;
        }}

        .es-p10 {{
            padding: 10px;
        }}

        .es-p10t {{
            padding-top: 10px;
        }}

        .es-p10b {{
            padding-bottom: 10px;
        }}

        .es-p10l {{
            padding-left: 10px;
        }}

        .es-p10r {{
            padding-right: 10px;
        }}

        .es-p15 {{
            padding: 15px;
        }}

        .es-p15t {{
            padding-top: 15px;
        }}

        .es-p15b {{
            padding-bottom: 15px;
        }}

        .es-p15l {{
            padding-left: 15px;
        }}

        .es-p15r {{
            padding-right: 15px;
        }}

        .es-p20 {{
            padding: 20px;
        }}

        .es-p20t {{
            padding-top: 20px;
        }}

        .es-p20b {{
            padding-bottom: 20px;
        }}

        .es-p20l {{
            padding-left: 20px;
        }}

        .es-p20r {{
            padding-right: 20px;
        }}

        .es-p25 {{
            padding: 25px;
        }}

        .es-p25t {{
            padding-top: 25px;
        }}

        .es-p25b {{
            padding-bottom: 25px;
        }}

        .es-p25l {{
            padding-left: 25px;
        }}

        .es-p25r {{
            padding-right: 25px;
        }}

        .es-p30 {{
            padding: 30px;
        }}

        .es-p30t {{
            padding-top: 30px;
        }}

        .es-p30b {{
            padding-bottom: 30px;
        }}

        .es-p30l {{
            padding-left: 30px;
        }}

        .es-p30r {{
            padding-right: 30px;
        }}

        .es-p35 {{
            padding: 35px;
        }}

        .es-p35t {{
            padding-top: 35px;
        }}

        .es-p35b {{
            padding-bottom: 35px;
        }}

        .es-p35l {{
            padding-left: 35px;
        }}

        .es-p35r {{
            padding-right: 35px;
        }}

        .es-p40 {{
            padding: 40px;
        }}

        .es-p40t {{
            padding-top: 40px;
        }}

        .es-p40b {{
            padding-bottom: 40px;
        }}

        .es-p40l {{
            padding-left: 40px;
        }}

        .es-p40r {{
            padding-right: 40px;
        }}

        .es-menu td {{
            border: 0;
        }}

        .es-menu td a img {{
            display: inline-block !important;
        }}

        /* END CONFIG STYLES */
        a {{
            font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;
            font-size: 15px;
            text-decoration: none;
        }}

        h1 {{
            font-size: 36px;
            font-style: normal;
            font-weight: bold;
            color: #333333;
        }}

        h1 a {{
            font-size: 36px;
            text-align: left;
        }}

        h2 {{
            font-size: 30px;
            font-style: normal;
            font-weight: bold;
            color: #333333;
        }}

        h2 a {{
            font-size: 30px;
            text-align: left;
        }}

        h3 {{
            font-size: 18px;
            font-style: normal;
            font-weight: bold;
            color: #333333;
        }}

        h3 a {{
            font-size: 18px;
            text-align: left;
        }}

        p,
        ul li,
        ol li {{
            font-size: 15px;
            font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;
            line-height: 150%;
        }}

        ul li,
        ol li {{
            Margin-bottom: 15px;
        }}

        .es-menu td a {{
            text-decoration: none;
            display: block;
        }}

        .es-wrapper {{
            width: 100%;
            height: 100%;
            background-image: ;
            background-repeat: repeat;
            background-position: center top;
        }}

        .es-wrapper-color {{
            background-color: #eeeeee;
        }}

        .es-content-body {{
            background-color: #ffffff;
        }}

        .es-content-body p,
        .es-content-body ul li,
        .es-content-body ol li {{
            color: #333333;
        }}

        .es-content-body a {{
            color: #ed8e20;
        }}

        .es-header {{
            background-color: transparent;
            background-image: ;
            background-repeat: repeat;
            background-position: center top;
        }}

        .es-header-body {{
            background-color: #044767;
        }}

        .es-header-body p,
        .es-header-body ul li,
        .es-header-body ol li {{
            color: #ffffff;
            font-size: 14px;
        }}

        .es-header-body a {{
            color: #ffffff;
            font-size: 14px;
        }}

        .es-footer {{
            background-color: transparent;
            background-image: ;
            background-repeat: repeat;
            background-position: center top;
        }}

        .es-footer-body {{
            background-color: #ffffff;
        }}

        .es-footer-body p,
        .es-footer-body ul li,
        .es-footer-body ol li {{
            color: #333333;
            font-size: 14px;
        }}

        .es-footer-body a {{
            color: #333333;
            font-size: 14px;
        }}

        .es-infoblock,
        .es-infoblock p,
        .es-infoblock ul li,
        .es-infoblock ol li {{
            line-height: 120%;
            font-size: 12px;
            color: #cccccc;
        }}

        .es-infoblock a {{
            font-size: 12px;
            color: #cccccc;
        }}

        a.es-button {{
            border-style: solid;
            border-color: #ed8e20;
            border-width: 15px 30px 15px 30px;
            display: inline-block;
            background: #ed8e20;
            border-radius: 5px;
            font-size: 16px;
            font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;
            font-weight: bold;
            font-style: normal;
            line-height: 120%;
            color: #ffffff;
            text-decoration: none;
            width: auto;
            text-align: center;
        }}

        .es-button-border {{
            border-style: solid solid solid solid;
            border-color: transparent transparent transparent transparent;
            background: #ed8e20;
            border-width: 0px 0px 0px 0px;
            display: inline-block;
            border-radius: 5px;
            width: auto;
        }}

        /* RESPONSIVE STYLES Please do not delete and edit CSS styles below. If you don't need responsive layout, please delete this section. */
        @media only screen and (max-width: 600px) {{

            p,
            ul li,
            ol li,
            a {{
                font-size: 16px !important;
                line-height: 150% !important;
            }}

            h1 {{
                font-size: 32px !important;
                text-align: left;
                line-height: 120% !important;
            }}

            h2 {{
                font-size: 26px !important;
                text-align: left;
                line-height: 120% !important;
            }}

            h3 {{
                font-size: 20px !important;
                text-align: left;
                line-height: 120% !important;
            }}

            h1 a {{
                font-size: 36px !important;
                text-align: left;
            }}

            h2 a {{
                font-size: 30px !important;
                text-align: left;
            }}

            h3 a {{
                font-size: 18px !important;
                text-align: left;
            }}

            .es-menu td a {{
                font-size: 16px !important;
            }}

            .es-header-body p,
            .es-header-body ul li,
            .es-header-body ol li,
            .es-header-body a {{
                font-size: 16px !important;
            }}

            .es-footer-body p,
            .es-footer-body ul li,
            .es-footer-body ol li,
            .es-footer-body a {{
                font-size: 16px !important;
            }}

            .es-infoblock p,
            .es-infoblock ul li,
            .es-infoblock ol li,
            .es-infoblock a {{
                font-size: 12px !important;
            }}

            *[class=""gmail-fix""] {{
                display: none !important;
            }}

            .es-m-txt-c,
            .es-m-txt-c h1,
            .es-m-txt-c h2,
            .es-m-txt-c h3 {{
                text-align: center !important;
            }}

            .es-m-txt-r,
            .es-m-txt-r h1,
            .es-m-txt-r h2,
            .es-m-txt-r h3 {{
                text-align: right !important;
            }}

            .es-m-txt-l,
            .es-m-txt-l h1,
            .es-m-txt-l h2,
            .es-m-txt-l h3 {{
                text-align: left !important;
            }}

            .es-m-txt-r img,
            .es-m-txt-c img,
            .es-m-txt-l img {{
                display: inline !important;
            }}

            .es-button-border {{
                display: inline-block !important;
            }}

            a.es-button {{
                font-size: 16px !important;
                display: inline-block !important;
                border-width: 15px 30px 15px 30px !important;
            }}

            .es-btn-fw {{
                border-width: 10px 0px !important;
                text-align: center !important;
            }}

            .es-adaptive table,
            .es-btn-fw,
            .es-btn-fw-brdr,
            .es-left,
            .es-right {{
                width: 100% !important;
            }}

            .es-content table,
            .es-header table,
            .es-footer table,
            .es-content,
            .es-footer,
            .es-header {{
                width: 100% !important;
                max-width: 600px !important;
            }}

            .es-adapt-td {{
                display: block !important;
                width: 100% !important;
            }}

            .adapt-img {{
                width: 100% !important;
                height: auto !important;
            }}

            .es-m-p0 {{
                padding: 0px !important;
            }}

            .es-m-p0r {{
                padding-right: 0px !important;
            }}

            .es-m-p0l {{
                padding-left: 0px !important;
            }}

            .es-m-p0t {{
                padding-top: 0px !important;
            }}

            .es-m-p0b {{
                padding-bottom: 0 !important;
            }}

            .es-m-p20b {{
                padding-bottom: 20px !important;
            }}

            .es-mobile-hidden,
            .es-hidden {{
                display: none !important;
            }}

            .es-desk-hidden {{
                display: table-row !important;
                width: auto !important;
                overflow: visible !important;
                float: none !important;
                max-height: inherit !important;
                line-height: inherit !important;
            }}

            .es-desk-menu-hidden {{
                display: table-cell !important;
            }}

            table.es-table-not-adapt,
            .esd-block-html table {{
                width: auto !important;
            }}

            table.es-social {{
                display: inline-block !important;
            }}

            table.es-social td {{
                display: inline-block !important;
            }}
        }}

        /* END RESPONSIVE STYLES */
        .es-p-default {{
            padding-top: 20px;
            padding-right: 35px;
            padding-bottom: 0px;
            padding-left: 35px;
        }}

        .es-p-all-default {{
            padding: 0px;
        }}
    </style>
</head>

<body>
    <div class=""es-wrapper-color"">
        <!--[if gte mso 9]>
			<v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t"">
				<v:fill type=""tile"" color=""#eeeeee""></v:fill>
			</v:background>
		<![endif]-->
        <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
            <tbody>
                <tr>
                    <td class=""esd-email-paddings"" valign=""top"">
                        <table class=""es-content esd-header-popover"" cellspacing=""0"" cellpadding=""0"" align=""center"">
                            <tbody>
                                <tr></tr>
                                <tr>
                                    <td class=""esd-stripe"" esd-custom-block-id=""7799"" align=""center"">
                                        <table class=""es-header-body"" style=""background-color: rgb(255, 255, 255);"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"">
                                            <tbody>
                                                <tr>
                                                    <td class=""esd-structure es-p20t es-p20b es-p20r es-p20l"" align=""left"" bgcolor=""transparent"" style=""background-color: transparent;"">
                                                        <!--[if mso]><table width=""560"" cellpadding=""0"" cellspacing=""0""><tr><td width=""410"" valign=""top""><![endif]-->
                                                        <table cellspacing=""0"" cellpadding=""0"" align=""left"" class=""es-left"">
                                                            <tbody>
                                                                <tr>
                                                                    <td class=""esd-container-frame es-m-p20b"" width=""410"" valign=""top"" align=""center"">
                                                                        <table width=""100%"" cellspacing=""0"" cellpadding=""0"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class=""esd-block-text es-m-txt-c es-p30t es-p20l"" align=""center"">
                                                                                        <h1 style=""color: #333333; line-height: 100%; text-align: center; font-family: 'open sans', 'helvetica neue', helvetica, arial, sans-serif;"">مصلحة الاحوال المدنية</h1>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align=""center"" class=""esd-block-text es-p20l"">
                                                                                        <p style=""line-height: 120%; font-size: 20px; color: #333333;""><strong>دولة ليبيا</strong></p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <!--[if mso]></td><td width=""20""></td><td width=""130"" valign=""top""><![endif]-->
                                                        <table cellpadding=""0"" cellspacing=""0"" class=""es-right"" align=""right"">
                                                            <tbody>
                                                                <tr>
                                                                    <td width=""130"" align=""left"" class=""esd-container-frame"">
                                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td align=""center"" class=""esd-block-image""><a target=""_blank""><img src=""https://drive.google.com/uc?export=view&id=1n6aPFQDxPhzKQvcp0p0nfVOrYzH4_x1T"" style=""width: 100px; max-width: 100%; height: 100px; display: block;"" alt width=""100""></a></td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                        <!--[if mso]></td></tr></table><![endif]-->
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"">
                            <tbody>
                                <tr>
                                    <td class=""esd-stripe"" align=""center"">
                                        <table class=""es-content-body"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"">
                                            <tbody>
                                                <tr>
                                                    <td class=""esd-structure es-p20b es-p20r es-p20l"" esd-custom-block-id=""7811"" align=""left"">
                                                        <table width=""100%"" cellspacing=""0"" cellpadding=""0"">
                                                            <tbody>
                                                                <tr>
                                                                    <td class=""esd-container-frame"" width=""560"" valign=""top"" align=""center"">
                                                                        <hr>
                                                                        <table width=""100%"" cellspacing=""0"" cellpadding=""0"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td class=""esd-block-text es-p10t es-p5b"" align=""left"">
                                                                                        <h3 style=""color: #333333; text-align: right;"">&nbsp;الي السيد/ة</h3>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class=""esd-block-text es-p5t es-p5b"" align=""left"">
                                                                                        <h3 style=""color: #333333; text-align: right;"">
                                                                                            {0}
                                                                                            حول 
                                                                                            {1}
                                                                                        </h3>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class=""esd-block-text es-p5t es-p10b"" align=""right"">
                                                                                        <p style=""font-size: 16px; color: rgb(119, 119, 119); line-height: 150%; text-align: right;""> {2} </p>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class=""esd-block-text es-p40t"" align=""right"">
                                                                                        <span style=""color: #333333; text-align: right;font-size=25px"">{3}</span><br/>
                                                                                           <span style=""font-size: 16px; color: #777777;"">{4}</span>
                                                                                    </td>
                                                                                </tr>
                                                                               
                                                                                <tr>
                                                                                    <td class=""esd-block-text"" align=""right"">
                                                                                        <p style=""font-size: 16px; color: #777777;"">{5}</p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table class=""es-content esd-footer-popover"" cellspacing=""0"" cellpadding=""0"" align=""center"">
                            <tbody>
                                <tr></tr>
                                <tr>
                                    <td class=""esd-stripe"" align=""center"">
                                        <table class=""es-header-body"" style=""background-color: rgb(236, 235, 235);"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ecebeb"" align=""center"">
                                            <tbody>
                                                <tr>
                                                    <td class=""esd-structure es-p5t es-p35r es-p35l"" align=""left"">
                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                            <tbody>
                                                                <tr>
                                                                    <td width=""530"" align=""left"" class=""esd-container-frame"">
                                                                        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
                                                                            <tbody>
                                                                                <tr>
                                                                                    <td align=""center"" class=""esd-block-text"" esd-links-color=""#8987cb"" esd-links-underline=""underline"">
                                                                                        <p style=""line-height: 120%; font-size: 20px; color: #666666;"">يمكن الوصول الي الموقع <span style=""font-size:16px;""><strong><a href=""www.google.com"" style=""color: #8987cb; text-decoration: underline; font-size: 16px;""> بالضغط هنا </a></strong></span></p>
                                                                                    </td>
                                                                                </tr>
                                                                            </tbody>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</body>
</html>", AdTypeName, newMessageDTO.Subject,newMessageDTO.Content, userSender.UserName, userSender.BranchName, fillAttach);
                
                var multipart = new Multipart("mixed");
                multipart.Add(bodyBuilder.ToMessageBody());
                messageEmail.Body = multipart;

                using (var clinet = new SmtpClient())
                {
                    await clinet.ConnectAsync("smtp.gmail.com", 587, false);
                    await clinet.AuthenticateAsync("mailkittest1@gmail.com", "P@$$w0rd123");
                    await clinet.SendAsync(messageEmail);
                    await clinet.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
               
            }
        }

        private async Task SendSMS(NewMessageDTO newMessageDTO,string AdTypeName)
        {

            try
            {
                
                string messageBody ="";
                List<string> phoneNumber = new List<string>();
                var body = this.help.GetPlainTextFromHtml(newMessageDTO.Content);
                string messageFile="";
                string intro = string.Format(@"وردك ({0}) تحت عنوان ({1})", AdTypeName,newMessageDTO.Subject);
                if(newMessageDTO.Files.Length>0)
                {
                    messageFile = "(يوجد لديك ملفات مرفقة)";
                }

                if((body.Length + intro.Length + messageFile.Length) >150)
                {
                    messageBody = intro + "\n" + messageFile;
                }
                else
                {
                    messageBody = intro + "\n" + body + "\n" + messageFile;
                }

                var users = await GetUsers();

                foreach (var item in newMessageDTO.Selectedusers)
                {
                    var user = users.SingleOrDefault(s => s.UserId == item);

                    phoneNumber.Add(user.Phone);

                }

                var send = await SMSHandler(messageBody, phoneNumber);

                if (send)
                {
                    // Update Send SMS Flag True
                }

            }
            catch (Exception)
            {

            }
        }

        private async Task<bool> SMSHandler(string messageBody,List<string> Phone)
        {
            try
            {
                MessageServicesClient messageServicesClient = new MessageServicesClient();
                var result=await messageServicesClient.SendSMSAsync(messageBody, Phone.ToArray());
                return true;
            }
            catch (Exception e)
            {
              
            }
            return false;

        }
         

        private async Task<List<UserSendEmailAndSMS>> GetUsers()
        {

            var userData = await db.Users.Where(w => w.Status != 9).Select(s => new UserSendEmailAndSMS
            {
                UserName = s.FullName,
                EmailUser = s.Email,
                UserId = s.UserId,
                BranchName = s.Branch.Name,
                Phone = s.Phone
            }).ToListAsync();

            return userData;
        }

        public class UserSendEmailAndSMS
        {
            public string UserName { get; set; }
            public string EmailUser { get; set; }
            public string BranchName { get; set; }
            public long UserId { get; set; }
            public string Phone { get; set; }
        }
       
    }
}