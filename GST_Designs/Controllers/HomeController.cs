using GST_Designs.DBAccess;
using GST_Designs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}