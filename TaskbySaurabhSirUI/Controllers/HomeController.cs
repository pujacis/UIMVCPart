using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI.Controllers
{
    
    public class HomeController : Controller
    {
        private string baseapi = "https://localhost:7063/api/";
        private static readonly HttpClient client = new HttpClient();
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
            return View();
        }
        public JsonResult GetStateById(int CountryId)
        {
            List<RepoWithState> states = new List<RepoWithState>();
            var statedata = _data.RepoWithStates.Where(x => x.CountryId == CountryId).ToList();            
            if (statedata != null)
            {
                foreach (var state in statedata)
                {
                    RepoWithState stat = new RepoWithState();
                    stat.StateName = state.StateName;
                    stat.StateId = state.StateId;

                    stat.CountryId = Convert.ToInt32(state.CountryId);
                    states.Add(stat);

                }

            }
            return Json(states);


        }
        public IActionResult GetCity(int stateid)
        {
            List<RepoWithCity> city = new List<RepoWithCity>();
           
            var CityList = _data.RepoWithCities.Where(x => x.StateId == stateid).ToList();
            if (CityList != null)
            {
                foreach (var cityda in CityList)
                {
                    RepoWithCity citydat = new RepoWithCity();
                    citydat.CityName = cityda.CityName;
                    citydat.CityId = cityda.CityId;
                    citydat.StateId = cityda.StateId;
                    city.Add(citydat);
                }


            }
            return Json(city);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person pr)
        {
            var files = HttpContext.Request.Form.Files;

            


            foreach (var Image 
                in files)
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
            using (var client = new HttpClient())
            {
                try
                {

                    var token = HttpContext.Session.GetString("accesstoken");
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                    request.Headers.Accept.Clear();
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    request.Content = new StringContent("{...}", Encoding.UTF8, "application/json");
                    var response = client.GetAsync("Person");
                    //  var response = await _client.SendAsync(request, CancellationToken.None);


                    List<Person> personlist = new List<Person>();
                   // client.BaseAddress = new Uri(baseapi + "Person");
                   // var response = client.GetAsync("Person");
                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<List<Person>>();
                        display.Wait();
                        personlist = display.Result;
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
                var files = HttpContext.Request.Form.Files;
                try
                {
                   
                    foreach (var Image
                        in files)
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


                    Person person = null;
                    client.BaseAddress = new Uri(baseapi+ "Person");
                    var response = client.GetAsync("Person/productId?personid=" + id);             
                  
                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<Person>();

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
        public IActionResult Editperson(Person Person)
        {
            using (var client = new HttpClient())
            {
                try
                {


                    //TblEmployee emp = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.PutAsJsonAsync<Person>("Person", Person);
                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetPerson","Home");
                    }
                    return View("GetPerson");
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

                    Person person = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.GetAsync("Person/personId?personid=" + id.ToString());
                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<Person>();

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
                    if (text.IsSuccessStatusCode)
                    {
                        return RedirectToAction("GetPerson");
                    }
                    return RedirectToAction("Deleteperson");
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

                    Person person = null;
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.GetAsync("Person/?personid=" + id.ToString());
                    response.Wait();
                    var text = response.Result;
                    if (text.IsSuccessStatusCode)
                    {
                        var display = text.Content.ReadAsAsync<Person>();

                        display.Wait();
                        person = display.Result;
                    }
                    return View();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }

        }


    }
}
//  https://localhost:7063/api/Person/9?personid=9
// https://localhost:7063/api/Person/{personId}