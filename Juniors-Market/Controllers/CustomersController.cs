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
            var alistofmarkets = await SearchFarmersMarkets();
            return View(alistofmarkets);
        }

        public async Task<List<MarketSearch>> SearchFarmersMarkets()
        {
            var aspUserId = User.Identity.GetUserId();
            var customer = context.Customer.Where(c => c.AspUserId == aspUserId).SingleOrDefault();

            List<MarketSearch> marketResult = new List<MarketSearch>();
            using (var client = new HttpClient())
            {
                var url = @"http://search.ams.usda.gov/farmersmarkets/v1/data.svc/zipSearch?zip=";
                url = url + customer.Zip;
                var response = await client.GetAsync(url);


                if (response.IsSuccessStatusCode)
                {
                    var markets = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(markets);
                    var j_marketId = json["results"][0]["id"];
                    var j_marketName = json["results"][0]["marketname"];
                    var marketId = j_marketId.ToObject<string>();
                    var marketName = j_marketName.ToObject<string>();

                    for (int i = 0; i < json["results"].Count(); i++)
                    {
                        MarketSearch marketlist = new MarketSearch();

                        marketlist.Id = json["results"][i]["id"].ToObject<string>();
                        var splitMarketname = json["results"][i]["marketname"].ToObject<string>();



                        marketlist.Marketname = splitMarketname;
                        marketResult.Add(marketlist);
                        context.SaveChanges();
                    }
                }
                return marketResult;
            }
        }

        public async Task<MarketDetail> GetMarketDetails(string id)
        {
            //pass the id. Get request to api to get market details
            MarketDetail marketDetails = new MarketDetail();
            using (var client = new HttpClient())
            {
                var url = @"http://search.ams.usda.gov/farmersmarkets/v1/data.svc/mktDetail?id=";
                url = url + id;
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var stringDetails = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(stringDetails);
                    var j_mAddress = json["marketdetails"]["Address"].ToObject<string>();
                    var j_mGoogleLink = json["marketdetails"]["GoogleLink"].ToObject<string>();
                    var j_mProducts = json["marketdetails"]["Products"].ToObject<string>();
                    var j_mSchedule = json["marketdetails"]["Schedule"].ToObject<string>();

                    marketDetails.Address = j_mAddress;
                    marketDetails.GoogleLink = j_mGoogleLink;
                    marketDetails.Products = j_mProducts;
                    marketDetails.Schedule = j_mSchedule;
                    context.SaveChanges(); 
                }
            }
            return marketDetails;
        }

        public async Task<ActionResult> DisplayMarketDetails()
        {
            var farmerMarketDetails = await GetMarketDetails();
            return View(farmerMarketDetails);
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
