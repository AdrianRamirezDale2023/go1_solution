
using PruebaIngreso.Models;
using Quote.Contracts;
using Quote.Models;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Web.Mvc;


namespace PruebaIngreso.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteEngine quote;
        private readonly IMarginProvider marginProvider;

        public HomeController(IQuoteEngine quote, IMarginProvider marginProvider)
        {
            this.quote = quote;
            this.marginProvider = marginProvider;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);
            var tour = result.Tours.FirstOrDefault();
            ViewBag.Message = "Test 1 Correcto";
            return View(tour);
        }

        public ActionResult Test2()
        {
            ViewBag.Message = "Test 2 Correcto";
            return View();
        }

        public async Task<ActionResult> Test3()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Test3(Test3Post test)
        {
            if (ModelState.IsValid) 
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var client = new RestClient(@"https://refactored-pancake.free.beeceptor.com/");
                var request = new RestRequest($"/margin/{test.Code}", Method.GET);
                var response = client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    ViewBag.Message = "{ \"margin\": 0.0}";
                }
                else
                {
                    ViewBag.Message = response.Content;
                }
            }
            

            return View(test);
        }

        public async Task<ActionResult> Test4()
        {
            var request = new TourQuoteRequest
            {
                adults = 1,
                ArrivalDate = DateTime.Now.AddDays(1),
                DepartingDate = DateTime.Now.AddDays(2),
                getAllRates = true,
                GetQuotes = true,
                RetrieveOptions = new TourQuoteRequestOptions
                {
                    GetContracts = true,
                    GetCalculatedQuote = true,
                },
                TourCode = "E-U10-PRVPARKTRF",
                Language = Language.Spanish
            };

            var result = this.quote.Quote(request);

            foreach(var tourQuote in result.TourQuotes) 
            {
                tourQuote.margin = await marginProvider.GetMarginAsync(tourQuote.ContractCode);
            }
            
            return View(result.TourQuotes);
        }
    }
}