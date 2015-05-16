using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDrive.Models;
using WebDrive.Service.Interface;
using WebDrive.Manager.QRCode;
using System.IO;

namespace WebDrive.Controllers
{
    [Authorize]
    public class QRCodeController : Controller
    {
        public ActionResult GetCodeImage(string codeString)
        {
            QRCodeManager qr = new QRCodeManager();
            MemoryStream ms = qr.GenerateQRCodeImage(codeString);
            return File(ms.ToArray(), "image/jpeg");
        }

        public ActionResult index()
        {
            return View();
        }
    }
}
