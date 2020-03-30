using GrabAPI.Models;
using GrabAPI.Models.ViewModel;
using GrabAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace GrabAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly AppDbContext appDbContext;
        public CartDetails cartDetails { get; set; }

        public CartController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public ActionResult<Response> Get()
        {

            int userId;

            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    userId = int.Parse(HttpContext.Session.GetString("userid"));
                    var cart = appDbContext.Carts.Where(c => c.UserId == userId);
                    User user = appDbContext.Users.Where(c => c.Id == userId).SingleOrDefault();
                    if (cart != null)
                    {
                        return new Response(cart, 200);
                    }
                    else
                    {
                        return new Response(null, 404, "Nothing is added to cart");
                    }

                }
                else
                {
                    return new Response(null, 403, "Access denied");
                }
            }
            catch (Exception ex)
            {
                return new Response(null, 500, ex.Message);
            }

        }

        [HttpPost]
        public ActionResult<Response> Post(Cart CartObj)
        {

            int userid = int.Parse(HttpContext.Session.GetString("userid"));

            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    if (ModelState.IsValid)
                    {

                        var cartFromDb = appDbContext.Carts.Where(c => c.UserId == userid
                                                        && c.ItemId == CartObj.ItemId).SingleOrDefault();
                        if (cartFromDb == null)
                        {
                            CartObj.UserId = userid;
                            User user = appDbContext.Users.Where(c => c.Id == userid).SingleOrDefault();
                            CartObj.User = user;
                            appDbContext.Carts.Add(CartObj);

                        }
                        else
                        {
                            cartFromDb.Count += CartObj.Count;
                            appDbContext.SaveChanges();
                            var updatedCart = appDbContext.Carts.Where(c => c.UserId == userid
                                                        && c.ItemId == CartObj.ItemId).SingleOrDefault();
                            return new Response(updatedCart, 200, "Cart updated");
                        }

                        appDbContext.SaveChanges();

                        return new Response(CartObj, 201, "Item Added to cart");
                    }
                    else
                    {
                        return new Response(null, 404, "Please make sure all fields are valid");
                    }

                }
                else
                {
                    return new Response(null, 403, "Access denied");
                }
            }
            catch (Exception ex)
            {
                return new Response(null, 500, ex.Message);
            }

        }

    }
}