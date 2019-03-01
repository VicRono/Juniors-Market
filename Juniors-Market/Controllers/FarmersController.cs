using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Juniors_Market.Models;
using Microsoft.AspNet.Identity;

namespace Juniors_Market.Controllers
{
    public class FarmersController : Controller
    {
        private ApplicationDbContext context;
        public FarmersController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Farmers
        public ActionResult Index()
        {
            var aspUserId = User.Identity.GetUserId();
            var farmer = context.Farmer.Where(c => c.AspUserId == aspUserId).SingleOrDefault();
            return View(farmer);
        }

        // GET: Farmers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = context.Farmer.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }

        // GET: Farmers/Create
        public ActionResult Create()
        {
            Farmer farmer = new Farmer();
            return View(farmer);
        }

        // POST: Farmers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MarketName,Address,City,State,Zip")] Farmer farmer)
        {
            Farmer newFarmer = new Farmer();
            newFarmer.MarketName = farmer.MarketName;
            newFarmer.Address = farmer.Address;
            newFarmer.City = farmer.City;
            newFarmer.State = farmer.State;
            newFarmer.Zip = farmer.Zip;
            newFarmer.AspUserId = farmer.AspUserId;

            context.Farmer.Add(newFarmer);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // GET: Farmers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = context.Farmer.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FarmerId,MarketName,Address,City,State,Zip,AspUserId")] Farmer farmer)
        {
            if (ModelState.IsValid)
            {
                context.Entry(farmer).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(farmer);
        }

        // GET: Farmers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Farmer farmer = context.Farmer.Find(id);
            if (farmer == null)
            {
                return HttpNotFound();
            }
            return View(farmer);
        }

        // POST: Farmers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Farmer farmer = context.Farmer.Find(id);
            context.Farmer.Remove(farmer);
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
