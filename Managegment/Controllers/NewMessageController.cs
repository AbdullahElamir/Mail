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

                switch (newMessageDTO.SelectedOption)
                {
                    case SelectedOption.All:
                        // EmIal and SMS
                        break;
                    case SelectedOption.Email:
                        //Email
                        await SendEmailToUsers(newMessageDTO);
                        break;
                    case SelectedOption.SMS:
                        //SMS
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

        private async Task<bool> SendEmailToUsers(NewMessageDTO newMessageDTO)
        {
            try
            {
                var AdTypeName =  db.AdTypes.SingleOrDefault(s => s.AdTypeId == newMessageDTO.Type).AdTypeName;
                var userData = await db.Users.Where(w => w.Status != 9).Select(s => new UserEmail
                {
                    UserName = s.FullName,
                    EmailUser = s.Email,
                    UserId = s.UserId,
                    BranchName=s.Branch.Name,
                    
                }).ToListAsync();
                foreach (var item in newMessageDTO.Selectedusers)
                {
                    var userSendEmail = userData.SingleOrDefault(w => w.UserId == item);
                    await SendEmail(userSendEmail,newMessageDTO, AdTypeName);
                }
                return true;

            }
            catch (Exception)
            {

                throw;
            }
           
        }
        private async Task SendEmail(UserEmail userEmail, NewMessageDTO newMessageDTO,string AdTypeName)
        {
            try
            {
                var messageEmail = new MimeMessage();
                messageEmail.From.Add(new MailboxAddress("مصلحة الاحوال المدانية", "nxn4ever@gmail.com"));
                messageEmail.To.Add(new MailboxAddress(userEmail.UserName,userEmail.EmailUser));
                messageEmail.Subject = newMessageDTO.Subject;
                var bodyBuilder = new BodyBuilder();

                var image1 = bodyBuilder.LinkedResources.Add(@"./Img/87001570133293444.png");
                image1.ContentId = MimeUtils.GenerateMessageId();
                bodyBuilder.HtmlBody = string.Format(@"<html
	style=""width:100%;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0;"">
    <head>
	<meta charset=""UTF-8"">
	<meta content=""width=device-width, initial-scale=1"" name=""viewport"">
	<meta name=""x-apple-disable-message-reformatting"">
	<meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
	<meta content=""telephone=no"" name=""format-detection"">
	<title>New email template 2019-10-03</title>
	<!--[if (mso 16)]><style type=""text/css"">     a {{text-decoration: none;}}     </style><![endif]-->
	<!--[if gte mso 9]><style>sup {{ font-size: 100% !important; }}</style><![endif]-->
	<!--[if !mso]><!-- -->
	<link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,400i,700,700i"" rel=""stylesheet"">
	<!--<![endif]-->
	<style type=""text/css"">
		@media only screen and (max-width:600px) {{

			p,
			ul li,
			ol li,
			a {{
				font-size: 16px !important;
				line-height: 150% !important
			}}

			h1 {{
				font-size: 32px !important;
				text-align: left;
				line-height: 120% !important
			}}

			h2 {{
				font-size: 26px !important;
				text-align: left;
				line-height: 120% !important
			}}

			h3 {{
				font-size: 20px !important;
				text-align: left;
				line-height: 120% !important
			}}

			h1 a {{
				font-size: 36px !important;
				text-align: left
			}}

			h2 a {{
				font-size: 30px !important;
				text-align: left
			}}

			h3 a {{
				font-size: 18px !important;
				text-align: left
			}}

			.es-menu td a {{
				font-size: 16px !important
			}}

			.es-header-body p,
			.es-header-body ul li,
			.es-header-body ol li,
			.es-header-body a {{
				font-size: 16px !important
			}}

			.es-footer-body p,
			.es-footer-body ul li,
			.es-footer-body ol li,
			.es-footer-body a {{
				font-size: 16px !important
			}}

			.es-infoblock p,
			.es-infoblock ul li,
			.es-infoblock ol li,
			.es-infoblock a {{
				font-size: 12px !important
			}}

			*[class=""gmail-fix""] {{
				display: none !important
			}}

			.es-m-txt-c,
			.es-m-txt-c h1,
			.es-m-txt-c h2,
			.es-m-txt-c h3 {{
				text-align: center !important
			}}

			.es-m-txt-r,
			.es-m-txt-r h1,
			.es-m-txt-r h2,
			.es-m-txt-r h3 {{
				text-align: right !important
			}}

			.es-m-txt-l,
			.es-m-txt-l h1,
			.es-m-txt-l h2,
			.es-m-txt-l h3 {{
				text-align: left !important
			}}

			.es-m-txt-r img,
			.es-m-txt-c img,
			.es-m-txt-l img {{
				display: inline !important
			}}

			.es-button-border {{
				display: inline-block !important
			}}

			a.es-button {{
				font-size: 16px !important;
				display: inline-block !important;
				border-width: 15px 30px 15px 30px !important
			}}

			.es-btn-fw {{
				border-width: 10px 0px !important;
				text-align: center !important
			}}

			.es-adaptive table,
			.es-btn-fw,
			.es-btn-fw-brdr,
			.es-left,
			.es-right {{
				width: 100% !important
			}}

			.es-content table,
			.es-header table,
			.es-footer table,
			.es-content,
			.es-footer,
			.es-header {{
				width: 100% !important;
				max-width: 600px !important
			}}

			.es-adapt-td {{
				display: block !important;
				width: 100% !important
			}}

			.adapt-img {{
				width: 100% !important;
				height: auto !important
			}}

			.es-m-p0 {{
				padding: 0px !important
			}}

			.es-m-p0r {{
				padding-right: 0px !important
			}}

			.es-m-p0l {{
				padding-left: 0px !important
			}}

			.es-m-p0t {{
				padding-top: 0px !important
			}}

			.es-m-p0b {{
				padding-bottom: 0 !important
			}}

			.es-m-p20b {{
				padding-bottom: 20px !important
			}}

			.es-mobile-hidden,
			.es-hidden {{
				display: none !important
			}}

			.es-desk-hidden {{
				display: table-row !important;
				width: auto !important;
				overflow: visible !important;
				float: none !important;
				max-height: inherit !important;
				line-height: inherit !important
			}}

			.es-desk-menu-hidden {{
				display: table-cell !important
			}}

			table.es-table-not-adapt,
			.esd-block-html table {{
				width: auto !important
			}}

			table.es-social {{
				display: inline-block !important
			}}

			table.es-social td {{
				display: inline-block !important
			}}
		}}

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
	</style>
</head>
<body style=""width:100%;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0;""> <div class=""es-wrapper-color"" style=""background-color:#EEEEEE;""> <!--[if gte mso 9]><v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t""> <v:fill type=""tile"" color=""#eeeeee""></v:fill> </v:background><![endif]-->
 <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top;""> <tr style=""border-collapse:collapse;""> <td valign=""top"" style=""padding:0;Margin:0;""> <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;""> <tr style=""border-collapse:collapse;""></tr> <tr style=""border-collapse:collapse;""> <td align=""center"" style=""padding:0;Margin:0;""> <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;"" width=""600"" cellspacing=""0"" cellpadding=""0"" align=""center""> <tr style=""border-collapse:collapse;"">
 <td align=""left"" style=""Margin:0;padding-left:10px;padding-right:10px;padding-top:15px;padding-bottom:15px;""> <!--[if mso]><table width=""580"" cellpadding=""0"" cellspacing=""0""><tr><td width=""282"" valign=""top""><![endif]--> <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left;""> <tr style=""border-collapse:collapse;""> <td width=""282"" align=""left"" style=""padding:0;Margin:0;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td class=""es-infoblock es-m-txt-c"" align=""left"" style=""padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC;"">
<p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:12px;font-family:arial, 'helvetica neue', helvetica, sans-serif;line-height:14px;color:#CCCCCC;""></p></td> </tr> </table></td> </tr> </table> <!--[if mso]></td><td width=""20""></td><td width=""278"" valign=""top""><![endif]--> <table class=""es-right"" cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right;""> <tr style=""border-collapse:collapse;""> <td width=""278"" align=""left"" style=""padding:0;Margin:0;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;"">
 <td class=""es-infoblock es-m-txt-c"" align=""right"" style=""padding:0;Margin:0;line-height:14px;font-size:12px;color:#CCCCCC;""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:12px;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:14px;color:#CCCCCC;""><a href=""http://#"" target=""_blank"" style=""-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:arial, 'helvetica neue', helvetica, sans-serif;font-size:12px;text-decoration:none;color:#CCCCCC;"">العرض علي المتصفح</a><br></p></td> </tr> </table></td> </tr> </table> <!--[if mso]></td></tr></table><![endif]--></td> </tr> </table></td> </tr> </table> <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;"">
 <tr style=""border-collapse:collapse;""></tr> <tr style=""border-collapse:collapse;""> <td align=""center"" style=""padding:0;Margin:0;""> <table class=""es-header-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#044767;"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#044767"" align=""center""> <tr style=""border-collapse:collapse;""> <td align=""left"" style=""padding:0;Margin:0;padding-top:20px;padding-left:35px;padding-right:35px;""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td width=""530"" align=""center"" valign=""top"" style=""padding:0;Margin:0;""> <table cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;"">
 <td align=""center"" style=""padding:0;Margin:0;""><img class=""adapt-img"" src=""cid:{0}"" alt style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;"" width=""155"" height=""152.79""></td> </tr> </table></td> </tr> </table></td> </tr> <tr style=""border-collapse:collapse;""> <td align=""left"" style=""Margin:0;padding-top:35px;padding-left:35px;padding-right:35px;padding-bottom:40px;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td width=""530"" valign=""top"" align=""center"" style=""padding:0;Margin:0;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td class=""es-m-txt-c"" align=""center"" style=""padding:0;Margin:0;"">
<h1 style=""Margin:0;line-height:36px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:36px;font-style:normal;font-weight:bold;color:#FFFFFF;"">{1}</h1></td> </tr> <tr style=""border-collapse:collapse;""> <td align=""center"" style=""padding:10px;Margin:0;""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:20px;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:24px;color:#FFFFFF;"">{2}</p></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;""> <tr style=""border-collapse:collapse;"">
 <td align=""center"" style=""padding:0;Margin:0;""> <table class=""es-content-body"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;""> <tr style=""border-collapse:collapse;""> <td align=""left"" style=""Margin:0;padding-bottom:25px;padding-top:35px;padding-left:35px;padding-right:35px;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td width=""530"" valign=""top"" align=""center"" style=""padding:0;Margin:0;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;"">
 <td align=""right"" style=""padding:0;Margin:0;padding-bottom:5px;padding-top:20px;""><h3 style=""Margin:0;line-height:22px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:18px;font-style:normal;font-weight:bold;color:#333333;""><br></h3></td> </tr> <tr style=""border-collapse:collapse;""> <td align=""right"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:15px;""> {3} </td> </tr> <tr style=""border-collapse:collapse;""> <td align=""left"" style=""padding:0;Margin:0;padding-top:5px;padding-bottom:10px;"">
</td> </tr> <tr style=""border-collapse:collapse;""> <td align=""right"" style=""padding:0;Margin:0;padding-top:5px;""></td> </tr> <tr style=""border-collapse:collapse;""> <td align=""right"" style=""padding:0;Margin:0;padding-top:40px;""><h3 style=""Margin:0;line-height:22px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:18px;font-style:normal;font-weight:bold;color:#333333;"">{4}</h3></td> </tr> <tr style=""border-collapse:collapse;""> <td align=""right"" style=""padding:0;Margin:0;""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-size:16px;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:24px;color:#777777;"">{5}</p></td> </tr> </table></td> </tr> </table></td> </tr> </table>
</td> </tr> </table> <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;""> <tr style=""border-collapse:collapse;""> <td align=""center"" style=""padding:0;Margin:0;""> <table class=""es-content-body"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;""> <tr style=""border-collapse:collapse;""> <td align=""left"" style=""padding:0;Margin:0;padding-top:15px;padding-left:35px;padding-right:35px;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td width=""530"" valign=""top"" align=""center"" style=""padding:0;Margin:0;"">
 <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td align=""center"" style=""padding:0;Margin:0;""><img src=""images/18501522065897895.png"" alt style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic;"" width=""46"" height=""22""></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;""> <tr style=""border-collapse:collapse;""> <td align=""center"" style=""padding:0;Margin:0;"">
 <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#1B9BA3;border-bottom:10px solid #48AFB5;"" width=""600"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#1b9ba3"" align=""center""> <tr style=""border-collapse:collapse;""> <td align=""left"" style=""padding:0;Margin:0;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td width=""600"" valign=""top"" align=""center"" style=""padding:0;Margin:0;""> <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr style=""border-collapse:collapse;""> <td style=""padding:0;Margin:0;"">
 <table class=""es-menu"" width=""40%"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;""> <tr class=""links-images-top"" style=""border-collapse:collapse;""> <td style=""Margin:0;padding-left:5px;padding-right:5px;padding-top:35px;padding-bottom:30px;border:0;"" id=""esd-menu-id-0"" width=""100%"" bgcolor=""transparent"" align=""center""><p style=""color:white;"">يرجب مراجعة البريد الالكتروني الخاص بالموقع</p> </td> </tr>
 </table></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table></td> </tr> </table> </div> </body></html>", image1.ContentId, AdTypeName, newMessageDTO.Subject, newMessageDTO.Content, userEmail.UserName, userEmail.BranchName);
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

        public class UserEmail
        {
            public string UserName { get; set; }
            public string EmailUser { get; set; }
            public string BranchName { get; set; }
            public long UserId { get; set; }

        }



        private void CreateHTMl()
        {
         

       


            //var attachment = new MimePart("image", "gif")
            //{
            //    Content = new MimeContent(File.OpenRead(path), ContentEncoding.Default),
            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            //    ContentTransferEncoding = ContentEncoding.Base64,
            //    FileName = Path.GetFileName(path)
            //};


          
        }
    }
}