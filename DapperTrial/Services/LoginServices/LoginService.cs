    using DapperTrial.Models.DataModel;
using DapperTrial.Models;
using DapperTrial.Models.ViewModel;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DapperTrial.Services.LoginServices
{
    public class LoginService : ILoginService
    {

        //private List<Person> people;
        //private readonly ApplicationDbContext _applicationDbContext;
        //private readonly ILogger<LoginService> _logger;
    
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;


        public LoginService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }

        public Users Login(string email, string password)
        {
                Users data = new Users();
                
            //using service dapper from startup.cs
                SqlConnection connection = new SqlConnection(_connectionString);
                //opening connection
            //sql query to select columns from table
                var sql = "select * from Users WHERE Email = @email and Password = @password";
                //storing lib value of table in data variable
                data = connection.QuerySingleOrDefault<Users>(sql, new { email = email, password = password });
                return data;
                //Console.WriteLine(data);
                ////read data 
                //data.Read();

                ////store data in new variable from each column from data table
                //var namedata = data["username"].ToString();
                //var passworddata = data["userpassword"].ToString();
                //var roledata = data["role"].ToString();
                //var iddata = data["userid"].ToString();





            
        }
    }
}
