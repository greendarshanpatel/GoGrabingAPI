using GrabAPI.Models;
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
    public class ItemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly AppDbContext appDbContext;

        public ItemController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        [HttpGet]
        public ActionResult<Response> GetProducts()
        {
            List<Item> items = new List<Item>();
            var itemsDB = appDbContext.Items.ToList();
            if (itemsDB != null)
            {
                foreach(var item in itemsDB)
                {
                    item.ItemType = appDbContext.ItemTypes.FirstOrDefault(i => i.Id == item.ItemTypeID);
                    item.Store = appDbContext.Stores.FirstOrDefault(s => s.Id == item.StoreId);

                    items.Add(item);
                }
                return new Response(items);
            }
            else
            {
                return new Response(null, 404, "There are no products");
            }

        }

        [HttpGet("{id}")]
        public ActionResult<Response> Get(int id)
        {

            var items = appDbContext.Items.FirstOrDefault(e => e.Id == id);
            if (items != null)
            {
                var itemType = appDbContext.ItemTypes.FirstOrDefault(i => i.Id == items.ItemTypeID);
                items.Store= appDbContext.Stores.FirstOrDefault(s => s.Id == items.StoreId);
                items.ItemType = itemType;
                return new Response(items, 200);
            }
            else
            {
                return new Response(null, 404, "Item with Id " + id.ToString() + " not found");
            }

        }

        [HttpPost]
        public ActionResult<Response> Post(Item item)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(null, 403, "Please login to create an item");
                }

                var userId = int.Parse(HttpContext.Session.GetString("userid"));
                User user = new User();

                user = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (!user.AccountType.Equals(2)){

                    return new Response(null, 403, "Please login as Admin to create an item");
                } 

                item.ItemType = appDbContext.ItemTypes.Where(ip => ip.Id == item.ItemTypeID).FirstOrDefault();
                item.Store = appDbContext.Stores.Where(s => s.Id == item.StoreId).FirstOrDefault();

                if (item.ItemType != null && item.Store != null)
                {
                    appDbContext.Items.Add(item);
                    appDbContext.SaveChanges();
                }
                else
                {
                    return new Response(item, 422, "Please fill all the required details and ensure they are correct");
                }
                return new Response(item, 201, "Item Created");
            }
            catch (Exception ex)
            {
                return new Response(null, 400, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Response> Delete(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(null, 403, "Please login to delete an item");
                }

                var userId = int.Parse(HttpContext.Session.GetString("userid"));
                User user = new User();

                user = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (!user.AccountType.Equals(2))
                {

                    return new Response(null, 403, "Please login as Admin to delete an item");
                }


                var item = appDbContext.Items.FirstOrDefault(e => e.Id == id);
                if (item == null)
                {

                    return new Response(null, 404, "Item with Id = " + id.ToString() + " not found to delete");
                }
                else
                {
                    appDbContext.Remove(item);
                    appDbContext.SaveChanges();
                    return new Response(null, 200, "The given item is deleted");

                }
            }
            catch (Exception ex)
            {
                return new Response(null, 400, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Response> Put(int id, Item item)
        {
            try
            {

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(null, 403, "Please login to update an item");
                }

                var userId = int.Parse(HttpContext.Session.GetString("userid"));
                User user = new User();

                user = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (!user.AccountType.Equals(2))
                {

                    return new Response(null, 403, "Please login as Admin to update an item");
                }


                var itemDb = appDbContext.Items.FirstOrDefault(e => e.Id == id);
                if (itemDb == null)
                {
                    return new Response(null, 404, "Item with Id "
                        + id.ToString() + " not found to update");
                }
                else
                {
                    item.ItemType = appDbContext.ItemTypes.Where(ip => ip.Id == item.ItemTypeID).FirstOrDefault();
                    item.Store = appDbContext.Stores.Where(s => s.Id == item.StoreId).FirstOrDefault();

                    if (item.ItemType != null && item.Store != null)
                    {
                        itemDb.Name = item.Name;
                        itemDb.ItemTypeID = item.ItemTypeID;
                        itemDb.Image = item.Image;
                        itemDb.Weight = item.Weight;
                        itemDb.Cost = item.Cost;
                        itemDb.StoreId = item.StoreId;

                        appDbContext.SaveChanges();
                        return new Response(itemDb, 200, "Item has been updated");
                    }
                    else
                    {
                        return new Response(item, 422, "Please fill all the required details and ensure they are correct");
                    }
                    
                }

            }
            catch (Exception ex)
            {    
                return new Response(null, 400, ex.Message);
            }
        }

    }
}