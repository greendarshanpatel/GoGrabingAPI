using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GrabAPI.Models;
using GrabAPI.Utilities;
using Microsoft.AspNetCore.Http;

namespace GrabAPI.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class StoreController : Controller
    {

        private readonly AppDbContext appDbContext;

        public StoreController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        [HttpGet]
        public ActionResult<Response> Get()
        {
            var stores = appDbContext.StoreAddresses.ToList();
            List<StoreAddress> indStore = new List<StoreAddress>();

            foreach(var store in stores)
            {
                store.Store = appDbContext.Stores.FirstOrDefault(s => s.Id == store.StoreID);
                indStore.Add(store);
            }

            return new Response(indStore, 200, "Stores with location");
        }

        [HttpGet("{id}")]
        public ActionResult<Response> GetStore(int id)
        {
            List<StoreAddress> storeLocations = new List<StoreAddress>();

           var storeLocationsFromDb = appDbContext.StoreAddresses.Where(s => s.StoreID == id).ToList();

            foreach (var store in storeLocationsFromDb)
            {
                store.Store = appDbContext.Stores.FirstOrDefault(s => s.Id == store.StoreID);

                storeLocations.Add(store);
            }

            return new Response(storeLocations, 200, "Store locations with the ID:" + id);
        }

        [HttpPost]
        public ActionResult<Response> Post(Store store)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(null, 403, "Please login to create store");
                }

                var userId = int.Parse(HttpContext.Session.GetString("userid"));
                User user = new User();

                user = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (!user.AccountType.Equals(2))
                {

                    return new Response(null, 403, "Please login as Admin to create store");
                }
        
                appDbContext.Stores.Add(store);
                appDbContext.SaveChanges();

                return new Response(store, 201, "Store has been created");

            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }
        }

        [HttpPost("StoreLocation")]
        public ActionResult<Response> PostAddress(StoreAddress location)
        {
            try
            {
                if (string.IsNullOrEmpty(HttpContext.Session.GetString("is_login")))
                {
                    return new Response(null, 403, "Please login to create store");
                }

                var userId = int.Parse(HttpContext.Session.GetString("userid"));
                User user = new User();

                user = appDbContext.Users.FirstOrDefault(u => u.Id == userId);

                if (!user.AccountType.Equals(2))
                {

                    return new Response(null, 403, "Please login as Admin to create store");
                }


                var storeFromDb = appDbContext.Stores.FirstOrDefault(s => s.Id == location.StoreID);
                
                if(storeFromDb == null)
                {
                    return new Response(null, 404, "The given store ID doesn't exist");
                }
                StoreAddress storeAddress = new StoreAddress
                {
                    Store = storeFromDb,
                    StoreID = location.StoreID,
                    Street = location.Street,
                    Unit = location.Unit,
                    PostalCode = location.PostalCode
                };
                appDbContext.StoreAddresses.Add(storeAddress);
                appDbContext.SaveChanges();

                return new Response(storeAddress, 201, "Store Location has been created");

            }
            catch (Exception ex)
            {
                return new Response(null, 404, ex.Message);
            }
        }
    }
}