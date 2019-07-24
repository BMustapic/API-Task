using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APITask.Models;
using Microsoft.AspNetCore.Mvc;

namespace APITask.Controllers
{
    public class BeerController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Index()
        {
            IEnumerable<Beer> beers = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.punkapi.com/v2/");
                //String query = Request.QueryString.ToString();

                String getString = "beers";

                var query = Request.Query;

                if (query.Count != 0)
                {
                    var queryList = new List<KeyValuePair<String, Microsoft.Extensions.Primitives.StringValues>>();

                    foreach (var q in query)
                    {
                        if (!String.IsNullOrEmpty(q.Value.ToString()))
                            queryList.Add(q);
                    }

                    if (queryList.Count != 0)
                    {
                        getString = String.Concat(getString, "?");

                        foreach (var q in queryList)
                        {
                            getString = String.Concat(getString, q.Key, "=", q.Value.ToString(), "&");
                        }

                        getString.Remove(getString.Length - 1);
                    }


                }
                //String getString = String.Concat("beers", query);

                Task<HttpResponseMessage> responseTask = client.GetAsync(getString);
                responseTask.Wait();

                HttpResponseMessage result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<Beer>> readTask = result.Content.ReadAsAsync<IList<Beer>>();
                    readTask.Wait();
                    beers = readTask.Result;
                }
                else
                {
                    beers = Enumerable.Empty<Beer>();

                    ModelState.AddModelError(String.Empty, "Server error");
                }
            }
            return View(beers);
        }

        public IActionResult Random()
        {
            Beer beer = null;

            Random random = new Random();
            Int32 randomNum = random.Next(1, 326);

            using (HttpClient client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://api.punkapi.com/v2/");

                Task<HttpResponseMessage> responseTask = client.GetAsync(String.Concat("beers/", randomNum.ToString()));
                responseTask.Wait();

                HttpResponseMessage result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    Task<IList<Beer>> readTask = result.Content.ReadAsAsync<IList<Beer>>();
                    readTask.Wait();
                    beer = readTask.Result.FirstOrDefault();
                }
                else
                {
                    beer = null;

                    ModelState.AddModelError(String.Empty, "Server error");
                }
            }

            return View(beer);
        }

    }
}