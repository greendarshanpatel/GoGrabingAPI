using GrabAPI.Models;
using GrabAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GrabAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly AppDbContext appDbContext;


        public UserController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet("GetAll")]
        public ActionResult<Response> GetAll()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {

                    var userId = int.Parse(HttpContext.Session.GetString("userid"));
                    User user = new User();

                    user = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

                    if (!user.AccountType.Equals(2))
                    {

                        return new Response(null, 403, "Please login as Admin to see all the users");
                    }

                    return new Response(appDbContext.Users.ToListAsync().Result);
                }
                else
                {
                    return new Response(null, 404, "Please login to see user details");
                }
            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }

        }

        [HttpGet]
        public ActionResult<Response> Get()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    var userId = int.Parse(HttpContext.Session.GetString("userid"));
                    User user = appDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();

                    return new Response(user, 200, "User details");
                }
                else
                {
                    return new Response(null, 404, "Authentication Failure");
                }
            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<Response> Post(User user)
        {
            try
            {
                User userFromDb = appDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
                if(userFromDb != null)
                {
                    return new Response(user, 409, "Email already exists");
                }

                user.Password = PasswordHandler.Encrypt(user.Password, "sblw-3hn8-sqoy19");
                appDbContext.Users.Add(user);
                appDbContext.SaveChanges();
                return new Response(user, 200, "User Created Successfully");
            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }
        }

        [HttpPut]
        public ActionResult<Response> Put(User user)
        {
            try
            {

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {

                    var userId = int.Parse(HttpContext.Session.GetString("userid"));
                    User userFromDb = appDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                    if (userFromDb != null)
                    {
                        userFromDb.Email = user.Email;
                        userFromDb.Name = user.Name;
                        user.Password = PasswordHandler.Encrypt(user.Password, "sblw-3hn8-sqoy19");
                        userFromDb.Password = user.Password;
                        userFromDb.ContactNumber = user.ContactNumber;
                        userFromDb.Unit = user.Unit;
                        userFromDb.Street = user.Street;
                        userFromDb.City = user.City;
                        userFromDb.PostalCode = user.PostalCode;
                        userFromDb.AccountType = userFromDb.AccountType;
                        appDbContext.SaveChanges();
                        return new Response(userFromDb);
                    }
                    else
                    {
                        return new Response(null, 404, "No Data Found for User");
                    }
                }
                else
                {
                    return new Response(null, 403, "Authentication Failure");
                }


            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }

        }

        [HttpDelete]
        public ActionResult<Response> Delete()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    var userId = int.Parse(HttpContext.Session.GetString("userid"));
                    User userFromDb = appDbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
                    if (userFromDb != null)
                    {
                        appDbContext.Users.Remove(userFromDb);
                        appDbContext.SaveChangesAsync();
                        return new Response(null);
                    }
                    else
                    {
                        return new Response(null, 404, "No Data Found for User");
                    }
                }
                else
                {
                    return new Response(null, 404, "Authentication Failure");
                }

            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }
        }

    }
}