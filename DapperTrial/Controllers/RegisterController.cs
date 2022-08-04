using Dapper;
using DapperTrial.Models.DataModel;
using DapperTrial.Services.LoginServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace DapperTrial.Controllers
{
    public class RegisterController : Controller
    {
        private ILoginService _loginService;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public RegisterController(ILoginService loginService, ILogger<LoginService> logger, IConfiguration configuration)
        {
            _loginService = loginService;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserRegisterRequest request, Users user)
        {
            Users data = new Users();
            SqlConnection connection = new SqlConnection(_connectionString);
            //opening connection
            connection.Open();
            //sql query to select columns from table
            var sql = "select * from Users where Email = @Email";
            data = connection.Query<Users>(sql, new { email = request.Email }).SingleOrDefault();
            if (data != null)
            {
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }

            CreatePasswordHash(request.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);


            //var newusersadge = new 
            //{
            //    Email = request.Email,
            //    PasswordHash = passwordHash,
            //    PasswordSalt = passwordSalt,
            //    VerificationToken = CreateRandomToken()
            //};
            string roleq = "undefined";

            string sqlQuery = "Insert into Users (Username, Password, Address, Phonenumber, Email, Role,) Values (@Username, @Password, @Address, @Phonenumber, @Email, @Role) ";

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Username", user.Username);
            parameters.Add("@Email", user.Email);
            parameters.Add("@Password", user.Password);
            parameters.Add("@Address", user.Address);
            parameters.Add("@Phonenumber", user.Phonenumber);
            parameters.Add("@Role", roleq);
            try
            {
                var runqry = connection.Execute(sqlQuery, parameters);
            }
            catch (Exception ex)
            { 
                
            }
            return RedirectToAction(controllerName: "Login", actionName: "Index");
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
