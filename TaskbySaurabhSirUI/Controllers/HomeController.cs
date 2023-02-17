using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI.Controllers
{

    public class HomeController : Controller
    {
        private string baseapi = "https://localhost:7063/api/";
        // private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _data;

        public HomeController(ILogger<HomeController> logger, DataContext data)
        {
            _logger = logger;
            _data = data;
        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult CreatePerson()
        {
            HttpClient client = new HttpClient();

            List<SelectListItem> cmlist = new List<SelectListItem>();
            client.BaseAddress = new Uri(baseapi + "CountryStoreproce");
            var response = client.GetAsync("CountryStoreproce");
            response.Wait();
            var text = response.Result;
            var display = text.Content.ReadAsStringAsync().Result;
            List<Countryddl> cddl = JsonConvert.DeserializeObject<List<Countryddl>>(display);
            cmlist.Add(new SelectListItem { Text = "-Select-Country-", Value = "" });

            foreach (var item in cddl)
            {
                SelectListItem cm = new SelectListItem();
                cm.Value = Convert.ToString(item.countryId);
                cm.Text = item.countryName;
                cmlist.Add(cm);
            }
            ViewBag.Con = cmlist;
            return View();
        }
        public JsonResult GetStateById(int CountryId)
        {
            HttpClient client = new HttpClient();
            List<RepoWithState> states = new List<RepoWithState>();
            // var statedata = _data.RepoWithStates.Where(x => x.CountryId == CountryId).ToList();
            List<SelectListItem> cmlist = new List<SelectListItem>();
            client.BaseAddress = new Uri(baseapi + "CountryStoreproce");
            //  https://localhost:7063/api/CountryStoreproce/getstatebycountry/1?countryid=1
            //"Person/productId?personid=" + id
            //https://localhost:7063/api/Person/1?personid=1
            var response = client.GetAsync("CountryStoreproce/getstatebycountry/Country?countryid=" + CountryId);
            response.Wait();
            var text = response.Result;
            var display = text.Content.ReadAsStringAsync().Result;
            List<Stateddl> Stateddl = JsonConvert.DeserializeObject<List<Stateddl>>(display);
            cmlist.Add(new SelectListItem { Text = "-Select-State-", Value = "" });

            if (Stateddl != null)
            {
                foreach (var state in Stateddl)
                {
                    RepoWithState stat = new RepoWithState();
                    stat.StateName = state.stateName;
                    stat.StateId = state.stateId;
                    states.Add(stat);

                }

            }
            return Json(states);


        }
        public IActionResult GetCity(int stateid)
        {
            HttpClient client = new HttpClient();
            List<RepoWithCity> city = new List<RepoWithCity>();

            //  var CityList = _data.RepoWithCities.Where(x => x.StateId == stateid).ToList();
            //https://localhost:7063/api/CountryStoreproce/1?Cityid=1
            List<SelectListItem> cmlist = new List<SelectListItem>();
            client.BaseAddress = new Uri(baseapi + "CountryStoreproce");
            var response = client.GetAsync("CountryStoreproce/stateid/?Cityid=" + stateid);
            response.Wait();
            var text = response.Result;
            var display = text.Content.ReadAsStringAsync().Result;
            List<Cityddl> citylist = JsonConvert.DeserializeObject<List<Cityddl>>(display);
            cmlist.Add(new SelectListItem { Text = "-Select-City-", Value = "" });
            if (citylist != null)
            {
                foreach (var cityda in citylist)
                {
                    RepoWithCity citydat = new RepoWithCity();
                    citydat.CityName = cityda.cityName;
                    citydat.CityId = cityda.cityId;
                    city.Add(citydat);
                }


            }
            return Json(city);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person pr)
        {
            HttpClient client = new HttpClient();
            var files = HttpContext.Request.Form.Files;
            foreach (var Image in files)
            {
                if (Image != null && Image.Length > 0)
                {
                    byte[] bytes;
                    using (var stream = new MemoryStream())
                    {
                        Image.CopyTo(stream);
                        bytes = stream.ToArray();
                    }


                    String base64file = Convert.ToBase64String(bytes);
                    pr.FileName = files[0].FileName;
                    pr.base64data = base64file;

                }
            }

            //var accesstoken = HttpContext.Session.GetString("accesstoken");
            client.BaseAddress = new Uri(baseapi + "Person");
            var response = client.PostAsJsonAsync<Person>("Person", pr);

            response.Wait();
            var text = response.Result;
            return RedirectToAction("GetPerson");
            // return View();
        }
        [HttpGet]
        public IActionResult GetPerson()
        {
            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
            using (var client = new HttpClient(handler))
            {
                try
                {
                    var token = HttpContext.Session.GetString("accesstoken");
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);


                    client.BaseAddress = new Uri(baseapi + "Person");
                    var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                    request.Headers.Accept.Clear();
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    request.Content = new StringContent("{...}", Encoding.UTF8, "application/json");
                    var response = client.GetAsync("Person");
                    //  var response = await _client.SendAsync(request, CancellationToken.None);
                    List<GetPerson> personlist = new List<GetPerson>();

                    response.Wait();
                    var text = response.Result;

                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<List<GetPerson>>();
                        display.Wait();
                        personlist = display.Result;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Account");
                    }


                    return View(personlist);
                }
                catch (Exception ex)
                {

                    return View();
                }
            }
        }


        [HttpGet]
        public IActionResult Editperson(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    List<SelectListItem> cmlist = new List<SelectListItem>();
                    var cList = _data.RepoWithCountries.ToList();
                    foreach (var item in cList)
                    {
                        SelectListItem cm = new SelectListItem();
                        cm.Value = Convert.ToString(item.CountryId);
                        cm.Text = item.CountryName;
                        cmlist.Add(cm);
                    }
                    ViewBag.Con = cmlist;

                    GetPerson person = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.GetAsync("Person/productId?personid=" + id);

                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<GetPerson>();

                        display.Wait();
                        person = display.Result;
                    }
                    return View(person);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }
        [HttpPost]
        public IActionResult Editperson(GetPerson Person)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var files = HttpContext.Request.Form.Files;




                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            byte[] bytes;
                            using (var stream = new MemoryStream())
                            {
                                Image.CopyTo(stream);
                                bytes = stream.ToArray();
                            }


                            String base64file = Convert.ToBase64String(bytes);
                            Person.FileName = files[0].FileName;
                            Person.base64data = base64file;


                        }
                    }
                    //////////////////


                    //TblEmployee emp = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.PutAsJsonAsync<GetPerson>("Person", Person);
                    response.Wait();
                    var text = response.Result;
                    if (!text.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Editperson", "Home");
                    }
                    return RedirectToAction("GetPerson");
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }

        public IActionResult Deleteperson(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    GetPerson person = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.GetAsync("Person/personId?personid=" + id.ToString());
                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<GetPerson>();

                        display.Wait();
                        person = display.Result;
                    }
                    return View(person);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }
        [HttpPost, ActionName("Deleteperson")]
        public ActionResult Deleteconfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    //TblEmployee emp = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.DeleteAsync("Person/?personid=" + id.ToString());
                    //https://localhost:7063/api/Person?personid=3
                    response.Wait();
                    var text = response.Result;
                    //if (!text.IsSuccessStatusCode)
                    //{
                    //    return RedirectToAction("GetPerson");
                    //}
                    return RedirectToAction("GetPerson");
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
        public IActionResult Details(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {

                    GetPerson person = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.GetAsync("Person/productId?personid=" + id);

                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<GetPerson>();

                        display.Wait();
                        person = display.Result;
                    }
                    return View(person);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }


    }
    public class Cityddl
    {
        public int cityId { get; set; }
        public string cityName { get; set; }
    }
    public class Stateddl
    {
        public int stateId { get; set; }
        public string stateName { get; set; }
    }

    public class Countryddl
    {
        public int countryId { get; set; }
        public string countryName { get; set; }
    }
}
//  https://localhost:7063/api/Person/9?personid=9
// https://localhost:7063/api/Person/{personId}