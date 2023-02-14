using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TaskbySaurabhSirUI.Models;

namespace TaskbySaurabhSirUI.Controllers
{
    public class AccountController : Controller
    {
        private string baseapi = "https://localhost:7063/api/";
        private readonly ILogger<HomeController> _logger;
        private static readonly HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Index(Login login)
        {//  'https://localhost:7063/api/Account/login' 
            client.BaseAddress = new Uri(baseapi + "Account/login");
              var response = client.PostAsJsonAsync<Login>("login", login);
               response.Wait();
            var text = response.Result;
          //  HttpContext.Session.SetString("accesstoken", text.Content.ToString());
            return RedirectToAction("CreatePerson", "Home");
            //return View();
        }
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Register(Register register)
        {//  'https://localhost:7063/api/Account/register' 
            client.BaseAddress = new Uri(baseapi + "Account/register");
            var response = client.PostAsJsonAsync<Register>("register", register);
            response.Wait();
            var text = response.Result;
            return RedirectToAction("Index");
            // return View();
        }
    }
}
