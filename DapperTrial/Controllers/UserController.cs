using DapperTrial.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Mail;
using System.Net;
using StackExchange.Redis;
using DapperTrial.Models.DataModel;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.SqlServer.Management.Smo.Mail;
using MimeKit;

namespace DapperTrial.Controllers
{
    public class UserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {

            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DapperConnection");
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(User user)
        {
            bool Status = false;
            string message = "";
            //
            // Model Validation 
            if (ModelState.IsValid)
            {

                #region //Email is already Exist 
                var isExist = IsEmailExist(user.Email);
                if (isExist>0)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate Activation Code 
                Guid ActivationCode = Guid.NewGuid();
                #endregion

                #region  Password Hashing 
                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword); //
                #endregion
                bool IsEmailVerified = false;
                bool TokenUsed = false;
                DateTime currentTime = DateTime.Now;
                DateTime x30MinsLater = currentTime.AddMinutes(30);
                
                #region Save to Database
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    //dc.Users.Add(user);
                    //dc.SaveChanges();
                    string roleq = "undefined";

                    string sqlQuery = "Insert into Users (Username, Password, Address, Phonenumber, Email, Role, IsEmailVerified, ActivationCode) Values (@Username, @Password, @Address, @Phonenumber, @Email, @Role, @IsEmailVerified, @ActivationCode) ";
                    string tokenqry = "Insert into TokenExpiry (Email, Token, ExpirationDate, TokenUsed) Values (@Email, @ActivationCode, @Date, @TokenUsed)";
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Username", user.Username);
                    parameters.Add("@Email", user.Email);
                    parameters.Add("@Password", user.Password);
                    parameters.Add("@Address", user.Address);
                    parameters.Add("@Phonenumber", user.Phonenumber);
                    parameters.Add("@Role", roleq);
                    parameters.Add("@IsEmailVerified", IsEmailVerified);
                    parameters.Add("@Date", x30MinsLater);
                    parameters.Add("@TokenUsed", TokenUsed);
                    parameters.Add("@ActivationCode", ActivationCode);
                    try
                    {
                        var runqry = db.Execute(sqlQuery, parameters);
                        var runtokenqry = db.Execute(tokenqry, parameters);
                    }
                    catch (Exception ex)
                    {

                    }
                    var url = Url.Action(action: "VerifyAccount", controller: "User", values: null, protocol: "https");
                    //Send Email to User
                    SendVerificationLinkEmail(user.Email, url, ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link " +
                        " has been sent to your email id:" + user.Email;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid Request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }
        [HttpGet]
        public ActionResult VerifyAccount(string token)
        {
            bool Status = false;
            using (IDbConnection dc = new SqlConnection(_connectionString))
            {
                //dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                // Confirm password does not match issue on save changes

                DateTime currentTime = DateTime.Now;
                var veracc = "Select * From TokenExpiry Where Token = @token and expirationDate>= @time";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@token", token);
                parameters.Add("@time", currentTime);

                var v = dc.Execute(veracc, parameters);
                //var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    bool state = true;
                    var veremail = "Update Users set IsEmailVerified = @true where ActivationCode = @token";
                    DynamicParameters parameters2 = new DynamicParameters();
                    parameters2.Add("@token", token);
                    parameters2.Add("@true", state);

                    var vere = dc.Execute(veremail, parameters2);

                    //v.IsEmailVerified = true;
                    //dc.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }




        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string url, string activationCode)
        {
           


            string verifyUrl = url + "?token=" + activationCode;


            var fromEmail = new MailAddress("test@microbankernepal.com.np"/*, "Dotnet Awesome"*/);
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Server@123"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<h3>Welcome @ViewBag.Username</h3>" +
                "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + verifyUrl + "'> Click here </a> ";

            var smtp = new SmtpClient
            {
                Host = "smtpout.asia.secureserver.net",
                Port = 80,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            //using (var message = new MailMessage(fromEmail, toEmail)
            //{

            //    Subject = subject,
            //    Body = body,
            //    IsBodyHtml = true,

            //})


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
