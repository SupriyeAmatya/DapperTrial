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
                if (isExist)
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

                #region Save to Database
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    //dc.Users.Add(user);
                    //dc.SaveChanges();
                    string roleq = "undefined";

                    string sqlQuery = "Insert into Users (Username, Password, Address, Phonenumber, Email, Role, IsEmailVerified, ActivationCode) Values (@Username, @Password, @Address, @Phonenumber, @Email, @Role, @IsEmailVerified, @ActivationCode) ";

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Username", user.Username);
                    parameters.Add("@Email", user.Email);
                    parameters.Add("@Password", user.Password);
                    parameters.Add("@Address", user.Address);
                    parameters.Add("@Phonenumber", user.Phonenumber);
                    parameters.Add("@Role", roleq);
                    parameters.Add("@IsEmailVerified", IsEmailVerified);
                    parameters.Add("@ActivationCode", ActivationCode);
                    try
                    {
                        var runqry = db.Execute(sqlQuery, parameters);
                    }
                    catch (Exception ex)
                    {

                    }
                    //Send Email to User
                    SendVerificationLinkEmail(user.Email, ActivationCode.ToString());
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
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (IDbConnection dc = new SqlConnection(_connectionString))
            {
                //dc.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                // Confirm password does not match issue on save changes
                var veracc = "Select * From Users Where ActivationCode = @id";

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", new Guid(id));


                var v = dc.Execute(veracc, parameters);
                //var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    var veremail = "Update ISEmailVerified = 0 where ActivationCode = @id";
                    DynamicParameters parameters2 = new DynamicParameters();
                    parameters.Add("@id", new Guid(id));


                    var vere = dc.Execute(veracc, parameters2);

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
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = Request.Scheme,
                Host = Request.Host.ToString(),
                //Path = $"/User/VerifyAccount/{activationCode}"
            };


            //var link = uriBuilder.Uri.AbsoluteUri;
            string verifyUrl = $"/User/VerifyAccount/" + activationCode.ToString();

            if (!verifyUrl.StartsWith("http:"))
                verifyUrl = "http://" + verifyUrl;
            Uri uri;
            Uri.TryCreate(verifyUrl, UriKind.RelativeOrAbsolute, out uri);
            //if (!Uri.TryCreate(verifyUrl, UriKind.Absolute, out uril))
            //{
            //    //Bad bad bad!
            //    var verifyUri = $"/User/VerifyAccount/" + activationCode.ToString();

            //}
            //var link = uriBuilder.Uri.AbsoluteUri.Replace(uriBuilder.Uri.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("test@microbankernepal.com.np"/*, "Dotnet Awesome"*/);
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "Server@123"; // Replace with actual password
            string subject = "Your account is successfully created!";

            string body = "<h3>Welcome @ViewBag.Username</h3>" +
                "<br/><br/>We are excited to tell you that your Dotnet Awesome account is" +
                " successfully created. Please click on the below link to verify your account" +
                " <br/><br/><a href='" + uri + "'>" + uri + "</a> ";
           

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

        private bool IsEmailExist(string email)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var emailexist = "Select * From Users where Email = @email";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@email", email);

                var runqry = db.Execute(emailexist, parameters);


                //var v = dc.Users.Where(a => a.Email == email).FirstOrDefault();
                //return v != null;
                return (runqry > 0);
            }
        }
    }


}
