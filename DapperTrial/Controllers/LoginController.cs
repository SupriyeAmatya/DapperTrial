using DapperTrial.Models.DataModel;
using DapperTrial.Models.ViewModel;
using DapperTrial.Services.LoginServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Web;


namespace DapperTrial.Controllers
{


    public class LoginController : Controller
    {
        private ILoginService _loginService;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        private readonly ILogger<LoginService> _logger;


        public LoginController(ILoginService loginService, ILogger<LoginService> logger, IConfiguration configuration)
        {
            _loginService = loginService;
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }

        public IActionResult Success()
        {
            IDbConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            return View();
        }



        //returns view of login page
        public IActionResult Index()
        {
            return View();
        }

        //send data from login page to database and also make changes
        [HttpPost]
        //login button click action method 
        public IActionResult Login(string email, string password)
        {
            Users data = _loginService.Login(email, password);

            if (data != null)
            {

                HttpContext.Session.SetString("Username", data.Username);
                return RedirectToAction("welcome");
            }
            else
            {
                ViewBag.msg = "Invalid Account";
                return View("Index");
            }
        }

        public IActionResult Welcome()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View("Welcome");
        }



        //request data from server
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Role");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult _ForgotPassword()
        {
            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
           

            var url = Url.Action(action: "ChangePassword", controller: "Login", values: null, protocol: "https");
            GenericResponseModel response = _loginService.FGPASS(model, url);

            return Json(new { code = response.status, message = response.message });


        }

        [HttpGet]
        public IActionResult ChangePassword(string token)
        {
            return View();
        }

        [HttpPost]
        //login button click action method 
        public IActionResult ChangePassword(string newpassword, string confirmpassword, string token)
        {
            return View();
        }

    }
}
