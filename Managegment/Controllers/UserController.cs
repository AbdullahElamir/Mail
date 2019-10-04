using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Managegment.Controllers;
using Managegment.Models;
using Management.LibyanObjects;

using Microsoft.AspNetCore.Mvc;

namespace Management.Controllers
{    [Produces("application/json")]
    [Route("api/admin/User")]
    public class UserController : Controller
    {
        private Helper help;
        public IActionResult Index()
        {
            return View();
        }
        private readonly MailSystemContext db;
       
        public UserController(MailSystemContext context)
        {
            this.db = context;  
            help = new Helper();
        }

       
        [HttpGet("GetUsers")]
        public IActionResult GetUsers(int pageNo, int pageSize,int branchId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                IQueryable<Users> UsersQuery;
               
                UsersQuery = (from p in db.Users
                               where  p.Status != 9
                               select p);
               
                var userBranch = UsersQuery.First().BranchId;

          
                //    long?[] CivilId = db.Offices.AsEnumerable().Where(x => issusId.ToList().Contains(x.OfficeIndexId))
                //.Select(r => (long?)r.OfficeId)
                //.ToArray();
               
                var UsersCount = (from p in UsersQuery
                                  select p).Count();
                var UsersList = (from p in UsersQuery 

                                 select new
                                 {
                                     LoginName = p.LoginName,
                                     Email = p.Email,
                                     FullName = p.FullName,
                                     BranchName = p.Branch.Name,
                                     DateOfBirth = p.DateOfBirth,
                                     Gender = p.Gender,
                                     Password = p.Password,
                                     UserId = p.UserId,
                                     Status = p.Status,
                                     Phone = p.Phone,
                                     userType=p.UserType,
                                     CreatedBy = p.CreatedBy,
                                     UserName = db.Users.Where(k => k.UserId == p.CreatedBy).SingleOrDefault().FullName,
                                     ModifiedBy = db.Users.Where(k => k.UserId == p.ModifiedBy).SingleOrDefault().FullName,
                                 }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { User = UsersList, count = UsersCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetUsersByLevel")]
        public IActionResult GetUsersByLevel(int pageNo, int pageSize, int BranchLevel)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                IQueryable<Users> UsersQuery;

                UsersQuery = (from p in db.Users
                              where p.Status != 9 && p.Branch.BranchLevel == BranchLevel
                              select p);

                var UsersCount = (from p in UsersQuery
                                  select p).Count();
                var UsersList = (from p in UsersQuery
                                 select new
                                 {
                                     LoginName = p.LoginName,
                                     Email = p.Email,
                                     FullName = p.FullName,
                                     BranchName = p.Branch.Name,
                                     DateOfBirth = p.DateOfBirth,
                                     Gender = p.Gender,
                                     Password = p.Password,
                                     UserId = p.UserId,
                                     Status = p.Status,
                                     Phone = p.Phone,
                                     userType = p.UserType,
                                     CreatedBy = p.CreatedBy,
                                     UserName = db.Users.Where(k => k.UserId == p.CreatedBy).SingleOrDefault().FullName,
                                     ModifiedBy = db.Users.Where(k => k.UserId == p.ModifiedBy).SingleOrDefault().FullName,
                                 }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { User = UsersList, count = UsersCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("GetUsersByBranch")]
        public IActionResult GetUsersByBranch(int pageNo, int pageSize, int BrancId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);
                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }
                IQueryable<Users> UsersQuery;

                UsersQuery = (from p in db.Users
                              where p.Status != 9 && p.BranchId == BrancId
                              select p);

                var UsersCount = (from p in UsersQuery
                                  select p).Count();
                var UsersList = (from p in UsersQuery
                                 select new
                                 {
                                     LoginName = p.LoginName,
                                     Email = p.Email,
                                     FullName = p.FullName,
                                     BranchName = p.Branch.Name,
                                     DateOfBirth = p.DateOfBirth,
                                     Gender = p.Gender,
                                     Password = p.Password,
                                     UserId = p.UserId,
                                     Status = p.Status,
                                     Phone = p.Phone,
                                     userType = p.UserType,
                                     CreatedBy = p.CreatedBy,
                                     UserName = db.Users.Where(k => k.UserId == p.CreatedBy).SingleOrDefault().FullName,
                                     ModifiedBy = db.Users.Where(k => k.UserId == p.ModifiedBy).SingleOrDefault().FullName,
                                 }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();

                return Ok(new { User = UsersList, count = UsersCount });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{UserId}/delete")]
        public IActionResult DeleteUser(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var User = (from p in db.Users
                            where p.UserId == UserId && p.Status != 9
                            select p).SingleOrDefault();

                if (User == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                User.Status = 9;
                db.SaveChanges();
                return Ok("تم الغاء المستخدم بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{UserId}/Activate")]
        public IActionResult Activate(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var User = (from p in db.Users
                            where p.UserId == UserId && p.Status != 9
                            select p).SingleOrDefault();

                if (User == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                User.Status = 1;
                db.SaveChanges();
                return Ok("تم العمليه بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("{UserId}/Deactivate")]
        public IActionResult Deactivate(long UserId)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var User = (from p in db.Users
                            where p.UserId == UserId && p.Status != 9
                            select p).SingleOrDefault();

                if (User == null)
                {
                    return NotFound("خــطأ : المستخدم غير موجود");
                }

                User.Status = 2;
                db.SaveChanges();
                return Ok("تم العمليه بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UsersObj user)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if ((DateTime.Now.Year - user.DateOfBirth.Year) < 18)
                {
                    return BadRequest("يجب ان يكون عمر المستخدم اكبر من 18");
                }

                var cLoginName = (from u in db.Users
                                  where u.LoginName == user.LoginName
                                  select u).SingleOrDefault();
                if (cLoginName != null)
                {
                    return BadRequest(" اسم الدخول موجود مسبقا");


                }

                var cPhone = (from u in db.Users
                              where u.Phone == user.Phone
                              select u).SingleOrDefault();
                if (cPhone != null)
                {
                    return BadRequest(" رقم الهاتف موجود مسبقا");
                }

                var cUser = (from u in db.Users
                             where u.Email == user.Email && u.Status != 9
                             select u).SingleOrDefault();

                if (cUser != null)
                {
                    if (cUser.Status == 0)
                    {
                        return BadRequest("هدا المستخدم موجود من قبل يحتاج الي تقعيل الحساب فقط");
                    }
                    if (cUser.Status == 1 || cUser.Status == 2)
                    {
                        return BadRequest("هدا المستخدم موجود من قبل يحتاج الي دخول فقط");
                    }
                }

                cUser = new Users();

                cUser.Phone = user.Phone;
                cUser.LoginName = user.LoginName;
                cUser.FullName = user.FullName;
                cUser.BranchId = user.BranchId;
                cUser.Email = user.Email;

                cUser.DateOfBirth = user.DateOfBirth;
                cUser.Gender = (short)user.Gender;
                cUser.LoginTryAttempts = 0;
                cUser.CreatedBy = userId;
                cUser.CreatedOn = DateTime.Now;
                cUser.BranchId= (short)user.BranchId;

                cUser.Password = Security.ComputeHash(user.Password, HashAlgorithms.SHA512, null);

                cUser.CreatedOn = DateTime.Now;

                //1- Active
                //2- locked
                //9- deleted not exist
                cUser.Status = 0;
                db.Users.Add(cUser);
                db.SaveChanges();
                return Ok("تم تسجيل المستخدم بنجاح , الحساب غير مفعل الأن");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("UploadImage")]
        public IActionResult UploadImage([FromBody] UsersObj user)
        {
            var userId = this.help.GetCurrentUser(HttpContext);

            if (userId <= 0 && userId == user.UserId)
            {
                return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
            }
            var Users = (from p in db.Users
                         where p.UserId == user.UserId
                         && (p.Status == 1 || p.Status == 2)
                         select p).SingleOrDefault();

            if (Users == null)
            {
                return BadRequest("عفوا هدا المستخدم غير موجود");
            }

            Users.Photo = Convert.FromBase64String(user.Photo.Substring(user.Photo.IndexOf(",") + 1));
            Users.ModifiedBy = userId;
            Users.ModifiedOn = DateTime.Now;
            db.SaveChanges();
            return Ok("تم تغير الصورة بنـجاح");

        }

        [HttpPost("EditUser")]
        public IActionResult EditUser([FromBody] UsersObj user)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Users = (from p in db.Users
                             where p.UserId == user.UserId
                             && (p.Status != 9)
                             select p).SingleOrDefault();

                if (Users == null)
                {
                    return BadRequest("خطأ بيانات المستخدم غير موجودة");
                }

                if (Users.Phone != user.Phone)
                {
                    var cPhone = (from u in db.Users
                                  where u.Phone == user.Phone
                                  select u).SingleOrDefault();
                    if (cPhone != null)
                    {
                        return BadRequest(" رقم الهاتف موجود مسبقا");




                    }

                }
                if (Users.Email != user.Email)
                {
                    var cUser = (from u in db.Users
                                 where u.Email == user.Email && u.Status != 9
                                 select u).SingleOrDefault();

                    if (cUser != null)
                    {
                        if (cUser.Status == 0)
                        {
                            return BadRequest("هدا المستخدم موجود من قبل يحتاج الي تقعيل الحساب فقط");
                        }
                        if (cUser.Status == 1 || cUser.Status == 2)
                        {
                            return BadRequest("هدا المستخدم موجود من قبل يحتاج الي دخول فقط");
                        }
                    }
                }
                // Users.LoginName = user.LoginName;
                Users.FullName = user.FullName;
                Users.Phone = user.Phone;
                Users.Email = user.Email;
                Users.DateOfBirth = user.DateOfBirth;
                Users.Gender = user.Gender;
                Users.BranchId = user.BranchId;
                Users.UserType = user.UserType;
                Users.ModifiedBy = userId;
                Users.ModifiedOn = DateTime.Now;

                db.SaveChanges();
                return Ok("تم تعديل بيانات المستخدم بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("EditUsersProfile")]
        public IActionResult EditUsersProfile([FromBody] UsersObj user)
        {
            try
            {
                var userId = this.help.GetCurrentUser(HttpContext);

                if (userId <= 0)
                {
                    return StatusCode(401, "الرجاء الـتأكد من أنك قمت بتسجيل الدخول");
                }

                var Users = (from p in db.Users
                             where p.UserId == userId
                             && (p.Status != 9)
                             select p).SingleOrDefault();

                if (Users == null)
                {
                    return BadRequest("خطأ بيانات المستخدم غير موجودة");
                }


                if (Users.Phone != user.Phone)
                {
                    var cPhone = (from u in db.Users
                                  where u.Phone == user.Phone
                                  select u).SingleOrDefault();
                    if (cPhone != null)
                    {
                        return BadRequest(" رقم الهاتف موجود مسبقا");




                    }

                }
                if (Users.Email != user.Email)
                {
                    var cUser = (from u in db.Users
                                 where u.Email == user.Email && u.Status != 9
                                 select u).SingleOrDefault();

                    if (cUser != null)
                    {
                        if (cUser.Status == 0)
                        {
                            return BadRequest("هدا المستخدم موجود من قبل يحتاج الي تقعيل الحساب فقط");
                        }
                        if (cUser.Status == 1 || cUser.Status == 2)
                        {
                            return BadRequest("هدا المستخدم موجود من قبل يحتاج الي دخول فقط");
                        }
                    }
                }

                Users.Email = user.Email;

                Users.Phone = user.Phone;

                Users.ModifiedBy = userId;
                Users.ModifiedOn = DateTime.Now;


                db.SaveChanges();
                return Ok("تم تعديل بيانات المستخدم بنجاح");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{UserId}/image")]
        public IActionResult GetUserImage(long UserId)
        {
            try
            {
                var UserImage = (from p in db.Users
                                 where p.UserId == UserId
                                 select p.Photo).SingleOrDefault();

                if (UserImage == null)
                {
                    return NotFound("المستخدم غير موجــود");
                }

                return File(UserImage, "image/jpeg");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


    }
}