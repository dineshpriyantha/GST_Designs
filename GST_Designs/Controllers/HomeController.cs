using GST_Designs.DBAccess;
using GST_Designs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace GST_Designs.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var cardData = new FileUpload();
            List<Card> cardLst = cardData.GetCardData();
            List<FileDetail> fileLst = cardData.GetFileData();

            ViewModel model = new ViewModel();

            model.GST_Card = cardLst;
            model.GST_FileDetail = fileLst;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string name, string email, string message)
        {
            // Create the mail messages
            MailMessage mail = new MailMessage();

            // set the addresses
            mail.From = new MailAddress("postmaster@gstteq.com");
            mail.To.Add("thimirathilakarathne@gmail.com");

            // set the content
            mail.Subject = "GST Designs";
            mail.Body = $"Name : {name} {Environment.NewLine}Email : {email} {Environment.NewLine}Message : {message}";

            // send the message
            SmtpClient smtp = new SmtpClient("mail.gstteq.com");
            NetworkCredential credential = new NetworkCredential("postmaster@gstteq.com", "Thimiragst@123");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;
            smtp.Port = 8889;
            smtp.EnableSsl = false;
            smtp.Send(mail);

            return RedirectToAction("Index");
        }
    }
}