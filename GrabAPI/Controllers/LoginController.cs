using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GrabAPI.Models;
using GrabAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace GrabAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly AppDbContext appDbContext;

       public LoginController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpPost]
        public ActionResult<Response> Post([FromBody] User user)
        {
            try
            {
                var fetched_user = appDbContext.Users.SingleOrDefaultAsync(p => p.Email == user.Email);
                if (fetched_user.Result != null && user.Password == PasswordHandler.Decrypt(fetched_user.Result.Password, "sblw-3hn8-sqoy19"))
                {
                    HttpContext.Session.SetString("is_login", "true");
                    HttpContext.Session.SetString("userid", fetched_user.Result.Id.ToString());
                    return new Response(null, 200, "Logged in succesfully");
                }
                else
                {
                    HttpContext.Session.SetString("is_login", "false");
                    return new Response(null, 200, "Login Failed");
                        
                }

            }
            catch (Exception ex)
            {      
                return new Response(null, 404, ex.Message);
            }
        }

    }
}