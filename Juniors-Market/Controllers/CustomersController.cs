﻿using System;
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
using Newtonsoft.Json;
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
        public async Task<ActionResult> Index()
        {
            var a = SearchFarmersMarkets();
            return View(await a);
        }

        public async Task<List<MarketSearch>> SearchFarmersMarkets()
        {
            //get user id, then use user id, find customer.then get zip code, get reequest to api with zipcode
            var aspUserId = User.Identity.GetUserId();
            var customer = context.Customer.Where(c => c.AspUserId == aspUserId).SingleOrDefault();

            MarketSearchResult marketResult = null;
            using (var client = new HttpClient())
            {
                var url = @"http://search.ams.usda.gov/farmersmarkets/v1/data.svc/zipSearch?zip=";
                url = url + customer.Zip;
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    marketResult = await response.Content.ReadAsAsync<MarketSearchResult>();
                }
            }

            if(marketResult != null)
            {
                return marketResult.Results;
            }

            throw new Exception("Error in market search!");

        }

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
            Customer customer = new Customer();
            return View(customer);
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerName,Address,City,State,Zip")] Customer customer)
        {
            customer.AspUserId = User.Identity.GetUserId();
            context.Customer.Add(customer);
            context.SaveChanges();

            return RedirectToAction("Index");


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
