using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Juniors_Market.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace Juniors_Market.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext context;
        public CustomersController()
        {
            context = new ApplicationDbContext();
        }
        // GET: Customers
        public ActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<List<MarketSearch>> SearchFarmersMarkets(string Zip)
        //{
        //    List<MarketSearch> marketSearch = new List<MarketSearch>();
        //    //get user id, then use user id, find customer. then get zip code, get reequest to api with zipcode
        //    //var aspUserId = User.Identity.GetUserId();
        //    //var newGuy = context.Customer.Where(c => c.AspUserId == aspUserId).SingleOrDefault();

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://search.ams.usda.gov/FarmersMarkets/v1/data.svc?wsdl");
        //        var response = await client.GetAsync($"/zipSearch?zip" +Zip);
        //        response.EnsureSuccessStatusCode();

        //        var stringResult = await response.Content.ReadAsStringAsync();
        //        var json = JObject.Parse(stringResult);
        //    }
        //    return marketSearch;
        //}

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerName,Address,City,State,Zip")] Customer customer)
        {
            Customer newGuy = new Customer();
            newGuy.CustomerName = customer.CustomerName;
            newGuy.Address = customer.Address;
            newGuy.City = customer.City;
            newGuy.State = customer.State;
            newGuy.Zip = customer.Zip;
            newGuy.AspUserId = customer.AspUserId;

            context.Customer.Add(newGuy);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");


            //return RedirectToAction("SearchFarmersMarkets", "Customers", newGuy.Zip);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerName,Address,City,State,Zip")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = context.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = context.Customer.Find(id);
            context.Customer.Remove(customer);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
