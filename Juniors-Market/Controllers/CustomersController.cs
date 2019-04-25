using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
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
            var aListOfMarkets = await SearchFarmersMarkets();
            return View(aListOfMarkets);
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
                    var j_marketId = json["results"][0]["id"].ToObject<string>();
                    var j_marketName = json["results"][0]["marketname"].ToObject<string>();

                    for (int i = 0; i < json["results"].Count(); i++)
                    {
                        MarketSearch marketlist = new MarketSearch();

                        marketlist.SearchId = json["results"][i]["id"].ToObject<string>();
                        //take string from json and remove distance and leave marketname
                        var splitMarketname = json["results"][i]["marketname"].ToObject<string>();
                        var subMarketName = splitMarketname.Substring(splitMarketname.IndexOf(' ') + 1);

                        marketlist.Marketname = subMarketName;
                        marketResult.Add(marketlist);
                        context.MarketSearch.Add(marketlist);
                        context.SaveChanges();
                    }
                }
                return marketResult;
            }
        }

        public async Task<ActionResult> GetMarketDetails(int id)
        {
            //pass the id.Get request to api to get market details
            var searchDetails = context.MarketSearch.Where(s => s.Id == id).FirstOrDefault();
            var exdeatils = context.MarketDetail.Where(i => i.SearchId == searchDetails.Id).FirstOrDefault();
            MarketDetailsViewModel detailsModel = new MarketDetailsViewModel
            {
                MarketSearch = new MarketSearch(),
                MarketDetail = new MarketDetail()
            };
            detailsModel.MarketSearch = searchDetails;
            using (var client = new HttpClient())
            {
                var url = @"http://search.ams.usda.gov/farmersmarkets/v1/data.svc/mktDetail?id=";
                url = url + searchDetails.SearchId;
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var stringDetails = await response.Content.ReadAsStringAsync();
                    var json = JObject.Parse(stringDetails);
                    var j_mAddress = json["marketdetails"]["Address"].ToObject<string>();

                    var j_mGoogleLink = json["marketdetails"]["GoogleLink"].ToObject<string>();
                    //get Lat and Long from google link
                    string getLatAndLong = j_mGoogleLink.Substring(j_mGoogleLink.LastIndexOf('/') + 4);
                    string[] splitLink = getLatAndLong.Split('(');
                    if (splitLink.Length > 0)
                    {
                        string val = splitLink[0];
                        int indexPercent = val.IndexOf("%");
                        string g_Latitude = val.Substring(0, indexPercent);

                        int lastPercent20 = val.LastIndexOf("%20");
                        string subLongitude = val.Substring(0, lastPercent20);

                        int firstPercent20 = subLongitude.IndexOf("-");
                        string g_Longitude = subLongitude.Substring(firstPercent20);
                    

                        var j_mProducts = json["marketdetails"]["Products"].ToObject<string>();

                        //keep schedule but remove everything to the right of AND including semicolon
                        var j_mSchedule = json["marketdetails"]["Schedule"].ToObject<string>();
                        var subSchedule = j_mSchedule.Substring(0, j_mSchedule.IndexOf(';'));

                        MarketDetail marketDetails = new MarketDetail();
                        marketDetails.SearchId = searchDetails.Id;
                        marketDetails.Address = j_mAddress;
                        marketDetails.Latitude = g_Latitude;
                        marketDetails.Longitude = g_Longitude;
                        marketDetails.Products = j_mProducts;
                        marketDetails.Schedule = subSchedule;

                        context.MarketDetail.Add(marketDetails);
                        context.SaveChanges();

                       detailsModel.MarketDetail = marketDetails;
                    }
                }
            }
            return View(detailsModel);
        }

        public ActionResult GetDirections(int id)
        {
            var aspUserId = User.Identity.GetUserId();
            var customer = context.Customer.Where(c => c.AspUserId == aspUserId).SingleOrDefault();
            var origin = customer.Address + " " + customer.City + " " + customer.State + " " + customer.Zip;

            var getMarketId = context.MarketSearch.Where(s => s.Id == id).FirstOrDefault();
            var marketSearchAndDeatils = context.MarketDetail.Where(i => i.SearchId == getMarketId.Id).FirstOrDefault();

            ViewBag.OriginAddress = origin;
            return View(marketSearchAndDeatils);
        }

        public ActionResult SendEmail(int id)
        {

            return View();
        }


        [HttpPost]
        public ActionResult SendEmail(Email email, string to, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("Nevin.Seibel.Test@gmail.com", "Juniors Markets");
                    var recieverEmail = new MailAddress(email.To);
                    var password = "donthackme1";
                    string body = "Hey! Check out this Farmers Market:http://localhost:4824/Customers/GetMarketDetails" + id + "";

                    var smtp = new SmtpClient()
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password),
                        Timeout = 20000
                    };
                    using (var mess = new MailMessage(senderEmail, recieverEmail))
                    {
                        mess.Subject = email.Subject;
                        mess.Body = body;
                        mess.IsBodyHtml = true;
                        smtp.Send(mess);
                    }
                }
        }
            catch (Exception)
            {
                ViewBag.Error = "Error sending e-mail";
            }
            return RedirectToAction("Index");
        }
        public ActionResult SaveMarket(int id)
        {
            MarketDetail SaveTrip = context.MarketDetail.Find(id);

            return View(SaveTrip);
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
