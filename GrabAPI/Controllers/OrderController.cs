using GrabAPI.Models;
using GrabAPI.Models.ViewModel;
using GrabAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrabAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AppDbContext appDbContext;
        public CartDetails cartDetails { get; set; }

        public OrderController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        [HttpGet]
        public ActionResult<Response> Get()
        {
            int userId;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
            {
                userId = int.Parse(HttpContext.Session.GetString("userid"));
                var user = appDbContext.Users.Where(c => c.Id == userId).SingleOrDefault();
                var orders = appDbContext.Orders.Where(o => o.UserId == userId).ToList();

                return new Response(orders, 200, "Orders placed by customer");
            }
            else
            {
                return new Response(null, 403, "Access denied");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Response> Get(int id)
        {
            int userId;

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
            {
                userId = int.Parse(HttpContext.Session.GetString("userid"));
                var user = appDbContext.Users.Where(c => c.Id == userId).SingleOrDefault();
                var order = appDbContext.Orders.Where(o => o.UserId == userId && o.Id == id).ToList();

                if (order.Count <= 0)
                {
                    return new Response(null, 404, "Given Order# doesn't exist");

                }

                return new Response(order, 200, "Selected Order");


            }
            else
            {
                return new Response(null, 403, "Access denied");
            }
        }

        [HttpPost]
        public ActionResult<Response> Post()
        {
            try
            {
                int userId;

                cartDetails = new CartDetails()
                {
                    Order = new Models.Order()
                };
                OrderDetails detailedOrder = new OrderDetails();

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    userId = int.Parse(HttpContext.Session.GetString("userid"));
                    var user = appDbContext.Users.Where(c => c.Id == userId).SingleOrDefault();

                    cartDetails.listCart = appDbContext.Carts.Where(c => c.UserId == userId).ToList();

                    cartDetails.Order.OrderDate = DateTime.Now;
                    cartDetails.Order.PaymentStatus = Static.StatusPaid;
                    cartDetails.Order.Status = Static.StatusSubmitted;
                    cartDetails.Order.User = user;
                    cartDetails.Order.UserId = userId;
                    cartDetails.Order.OrderTotal = 0;
                    List<OrderDetails> orderDetailsList = new List<OrderDetails>();
                    appDbContext.Orders.Add(cartDetails.Order);
                    appDbContext.SaveChanges();

                    if (cartDetails.listCart.Count <= 0)
                    {
                        return new Response(null, 404, "There are no Items added to cart");
                    }
                    else
                    {
                        foreach (var item in cartDetails.listCart)
                        {
                            item.Item = appDbContext.Items.Where(c => c.Id == item.ItemId).SingleOrDefault();
                            item.Store = appDbContext.Stores.Where(s => s.Id == item.StoreId).SingleOrDefault();
                            OrderDetails orderDetails = new OrderDetails();
                            orderDetails.ItemId = item.ItemId;
                            orderDetails.OrderId = cartDetails.Order.Id;
                            orderDetails.Price = item.Item.Cost;
                            orderDetails.Count = item.Count;
                            orderDetails.StoreId = item.StoreId;

                            cartDetails.Order.OrderTotal += orderDetails.Count * orderDetails.Price;
                            appDbContext.OrderDetails.Add(orderDetails);
                        }

                        appDbContext.SaveChanges();
                        appDbContext.Carts.RemoveRange(cartDetails.listCart);
                        appDbContext.SaveChanges();
                    }
                    return new Response(cartDetails.Order, 200, "Order has been submitted");
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