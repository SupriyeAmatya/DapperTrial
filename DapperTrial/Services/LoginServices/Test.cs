//using System.Net;
//using System.Net.Mail;
//using Microsoft.AspNetCore.Http.Extensions;
//namespace DapperTrial.Services.LoginServices
//{
//    public abstract class Test
//    {
//        public abstract Microsoft.AspNetCore.Http.HttpRequest Request { get; }

//        public static void SendResetLinkEmail(string emailID, string resettoken)
//        {

//            //try
//            //{
//            //    string a = null;
//            //    a.ToString();
//            //}
//            //catch (NullReferenceException e)
//            //{
//            //    //Code to do something with e
//            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");

//            var url = location.AbsoluteUri;
//            var fromEmail = new MailAddress("test@microbankernepal.com.np"/*, "Dotnet Awesome"*/);
//            var toEmail = new MailAddress(emailID);
//            var fromEmailPassword = "Server@123"; // Replace with actual password
//            string subject = "Email Change";
//            string body = "<h3>Hello @ViewBag.Username</h3>" +
//                "<br/><br/>Change Your Email";
//            //" successfully created. Please click on the below link to verify your account" +
//            //" <br/><br/><a href='" + uri + "'>" + uri + "</a> ";


//            var smtp = new SmtpClient
//            {
//                Host = "smtpout.asia.secureserver.net",
//                Port = 80,
//                EnableSsl = false,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
//            };



//            var message = new MailMessage(fromEmail, toEmail);

//            message.Subject = subject;
//            message.Headers.Add("Message-ID",
//                         String.Format("<{0}@{1}>",
//                         Guid.NewGuid().ToString(),
//                         "smtpout.asia.secureserver.net"));
//            message.Body = body;
//            message.IsBodyHtml = true;



//            smtp.Send(message);
//        }

//    }
//}
