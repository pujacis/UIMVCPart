using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI.Controllers
{
    public class AccountController : Controller
    {
        private string baseapi = "https://localhost:7063/api/";
        private readonly ILogger<HomeController> _logger;
        // private static readonly HttpClient client = new HttpClient();
        public IActionResult Index()
        {

            return View();
        }
        //public ActionResult LogOut()
        //{
        //    FormsAuthentication.SignOut();
        //    Session.Abandon(); // it will clear the session at the end of request
        //    return RedirectToAction("index", "main");
        //}
        [HttpPost]
        public IActionResult Index(Login login)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseapi + "Account/login");
            var response = client.PostAsJsonAsync<Login>("login", login);
            response.Wait();
            var text = response.Result;
            ViewBag.error = "";
           // TempData["name"] = "Invalid User";
            if(text.IsSuccessStatusCode)
            {
                var token = response.Result.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrWhiteSpace(token))
                {
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(token);
                    HttpContext.Session.SetString("accesstoken", myDeserializedClass.token);
                }
            }
            else
            {
                ViewBag.error = "Invlid Credentials";
                return View();
            }
          
          

            return RedirectToAction("CreatePerson", "Home");
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseapi + "Account/register");
            var response = client.PostAsJsonAsync<Register>("register", register);
            response.Wait();
            var text = response.Result;
            return RedirectToAction("Index");
            // return View();
        }
        public ActionResult logout()
        {
            HttpContext.Session.SetString("accesstoken", "");
            return RedirectToAction("Index");
        }

    }
    public class Root
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
