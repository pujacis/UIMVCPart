using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI.Controllers
{
    
    public class HomeController : Controller
    {
        private string baseapi = "https://localhost:7063/api/";
        private readonly ILogger<HomeController> _logger;
        private static readonly HttpClient client = new HttpClient();


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePerson(Person pr)
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
                    List<Person> personlist = new List<Person>();
                    client.BaseAddress = new Uri(baseapi + "Person");
                    var response = client.GetAsync("Person");
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
        public IActionResult Editperson(int id)
        {
            using (var client = new HttpClient())
            {
                try
                {
               // https://localhost:7063/api/Person/{personId}

                    Person person = null;
                    client.BaseAddress = new Uri(baseapi+ "Person");
                    var response = client.GetAsync("Person/personId?id=" + id);
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
                    return View("Editperson");
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