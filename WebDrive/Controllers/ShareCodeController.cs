using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDrive.Models;
using WebDrive.DAL.Context;
using WebDrive.Manager.Account;
using WebDrive.Manager.Generator;
using WebDrive.Service.Interface;

namespace WebDrive.Controllers
{
    public class ShareCodeController : Controller
    {
        private readonly IShareCodeService _shareCodeService;

        public ShareCodeController(IShareCodeService shareCodeService)
        {
            this._shareCodeService = shareCodeService;
        }

        [HttpPost]
        public PartialViewResult _DisplayShareCode(int shareID)
        {
            ViewBag.shareID = shareID;
            string validateString = this._shareCodeService.CreateCodeByShareID(ViewBag.shareID);
            ViewBag.validateString = validateString;
            return PartialView();
        }

    }
}