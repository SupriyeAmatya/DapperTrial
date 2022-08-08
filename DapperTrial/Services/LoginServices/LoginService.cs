using DapperTrial.Models.DataModel;
using DapperTrial.Models;
using DapperTrial.Models.ViewModel;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace DapperTrial.Services.LoginServices
{
    public class LoginService : ILoginService
    {

        //private List<Person> people;
        //private readonly ApplicationDbContext _applicationDbContext;
        //private readonly ILogger<LoginService> _logger;

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public  Microsoft.AspNetCore.Http.HttpRequest Request { get; }

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
     
            password = Crypto.Hash(password);
            var sql = "select * from Users WHERE Email = @email and Password = @password";
     
            data = connection.QuerySingleOrDefault<Users>(sql, new { email = email, password = password });
            return data;
           

        }

        public GenericResponseModel FGPASS(ForgotPasswordViewModel model, string url)
        {

            GenericResponseModel grm = new GenericResponseModel();

            var isExist = IsEmailExist(model.Email);
            if (isExist > 0)
            {
                Guid resettoken = Guid.NewGuid();
                SqlConnection connection = new SqlConnection(_connectionString);

                
                var sql = "Update Users set PasswordResetToken = @passresettoken where Email = @email";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@passresettoken", resettoken);
                parameters.Add("@email", model.Email);
                var runqry = connection.QuerySingleOrDefault<Users>(sql, parameters);
                SendResetLinkEmail(model.Email, url, resettoken.ToString());
                grm.status = 0;
                grm.message = "Success";
                return grm;
            }
            grm.status = 8;
            grm.message = "Failed";
            return grm;
        }


        //public ChangePasswordViewModel changePassword(string newpassword, string oldpassword)
        //{



        //    SqlConnection connection = new SqlConnection(_connectionString);

        //    newpassword = Crypto.Hash(newpassword);
        //    var sql = "Update Users set Password = @newpassword where PasswordResetToken=@token";


        //}

        public void SendResetLinkEmail(string emailID, string url,  string resettoken)
        {

            //try
            //{
            //    string a = null;
            //    a.ToString();
            //}
            //catch (NullReferenceException e)
            //{
            //    //Code to do something with e
            //}

            string finalurl = url + "?token=" + resettoken;
            var fromEmail = new MailAddress("test@microbankernepal.com.np"/*, "Dotnet Awesome"*/);
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Server@123"; // Replace with actual password
            string subject = "Email Change";
            string body = "<h3>Hello @ViewBag.Username</h3>" +
                "<br/><br/>Change Your Email" +
            " Use this link to change your email: " +
            " <br/><br/><a href='" + finalurl + "'> Click here </a> ";


            var smtp = new SmtpClient
            {
                Host = "smtpout.asia.secureserver.net",
                Port = 80,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };



            var message = new MailMessage(fromEmail, toEmail);

            message.Subject = subject;
            message.Headers.Add("Message-ID",
                         String.Format("<{0}@{1}>",
                         Guid.NewGuid().ToString(),
                         "smtpout.asia.secureserver.net"));
            message.Body = body;
            message.IsBodyHtml = true;



            smtp.Send(message);
        }
        private int IsEmailExist(string email)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var emailexist = "Select Count(*) From Users where Email = @email";
                DynamicParameters parameters2 = new DynamicParameters();
                parameters2.Add("@email", email);

                int runqry = db.Query<int>(emailexist, parameters2).SingleOrDefault();


                return runqry;
            }
        }
    }
}
