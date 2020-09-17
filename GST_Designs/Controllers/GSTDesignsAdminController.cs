using GST_Designs.DBAccess;
using GST_Designs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace GST_Designs.Controllers
{
    public class GSTDesignsAdminController : Controller
    {

        //[Authorize(Roles = "Admin")]
        //[HttpGet]
        public ActionResult Admin()
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("Login");
            }

            ViewBag.Message = "Your contact page.";

            return View(GetViewModel());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Admin(Card support)
        {
            var cardData = new FileUpload();
            int result = cardData.DeleteRecord(support);
            return View(GetViewModel());
        }

        [Authorize(Roles = "Admin")]
        public ViewModel GetViewModel()
        {
            var cardData = new FileUpload();
            List<Card> cardLst = cardData.GetCardData();
            List<FileDetail> fileLst = cardData.GetFileData();

            ViewModel model = new ViewModel();
            model.GST_Card = cardLst;
            model.GST_FileDetail = fileLst;
            return model;
        }

        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Login(User model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = new User() { Email = model.Email, Password = model.Password };

            user = Repository.GetUserDetails(user);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(model.Email, false);

                var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.Roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                return RedirectToAction("Admin");
            }

            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("Login");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Card support)
        {
            if (ModelState.IsValid)
            {
                List<FileDetail> fileDetails = new List<FileDetail>();
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);

                        var allowedExtensions = new[] { ".jpg", ".png" };
                        var checkExtention = Path.GetExtension(file.FileName).ToLower();

                        if (allowedExtensions.Contains(checkExtention))
                        {
                            FileDetail fileDetail = new FileDetail()
                            {
                                FileName = fileName,
                                Extension = Path.GetExtension(fileName),
                                Id = Guid.NewGuid()
                            };

                            fileDetails.Add(fileDetail);

                            //var path = Path.Combine(Server.MapPath("/Content/assets/img/admin/"), fileDetail.Id + fileDetail.Extension);
                            var path = HostingEnvironment.MapPath(Path.Combine("/Content/assets/img/admin/", fileDetail.Id + fileDetail.Extension));

                            file.SaveAs(path);


                            support.FileDetails = fileDetails;

                            FileUpload fileUpload = new FileUpload();
                            fileUpload.SaveFileDetails(support);



                            return RedirectToAction("Admin");
                        }
                        else
                        {
                            ViewBag.Message = " Please select a valid format (.jpg, .png). ";
                            ViewBag.Class = "alert alert-danger";
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Please add a image file ";
                        ViewBag.Class = "alert alert-danger";
                        return View();
                    }
                }

            }

            return View(support);
        }

    }
}