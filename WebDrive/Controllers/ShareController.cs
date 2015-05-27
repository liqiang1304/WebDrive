﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDrive.Filters;
using WebDrive.Models;
using WebDrive.Service.Interface;
using WebDrive.ViewModels;
using WebMatrix.WebData;
using WebDrive.Manager.QRCode;
using System.IO;

namespace WebDrive.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ShareController : Controller
    {
        private readonly IShareService _shareService;
        private readonly IUserFileService _userFileService;
        private readonly IRecoderService _recoderService;

        public ShareController(IShareService shareService, IUserFileService userFileService, IRecoderService recoderService)
        {
            this._shareService = shareService;
            this._userFileService = userFileService;
            this._recoderService = recoderService;
        }

        [HttpGet]
        public ActionResult CreateShare(int userFileID)
        {
            ViewBag.userFileID = userFileID;
            return View();
        }

        [HttpPost]
        public ActionResult CreateShare(ShareModels shareModels, int userFileID)
        {
            int userID = WebSecurity.CurrentUserId;
            UserFile userFile = this._userFileService.GetByID(userFileID);
            if (userFile != null) {
                Share share = this._shareService.CreateShare(shareModels, userFile.RealFileID, userID, userFile.FileName, userFile.FileType);
                return RedirectToAction("ShowShare", share);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ShowShare(Share share)
        {
            ViewBag.share = share;
            ViewBag.codeString = share.SharedQRCode;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetShare(string codeString)
        {
            Share share = this._shareService.GetByCodeString(codeString);
            if (share != null)
            {
                ViewBag.Password = (share.SharedType & ShareModels.PASSWORDNEED) > 0 ? true : false;
                ViewBag.codeString = codeString;
                return View();
            }
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetShare(string codeString, string password)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ShowShareInfo()
        {
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ShowShareInfo(string codeString, string password)
        {
            int userID = WebSecurity.CurrentUserId;
            IResult result = this._shareService.ValidateShare(codeString, userID, password);
            ViewBag.success = result.Success;
            ViewBag.codeString = codeString;
            ViewBag.password = password;
            if (result.Success)
            {
                Recoder recoder = this._recoderService.CreateRecoder(10);
                ViewBag.recoderString = recoder.RecoderString;
            }
            Share share = this._shareService.GetByCodeString(codeString);
            ViewBag.share = share;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetQRCodeShare()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetQRCodeShare(HttpPostedFileBase file)
        {
            QRCodeManager qr = new QRCodeManager();
            string codeString = qr.GetCodeString(file);
            if (codeString != null)
            {
                return RedirectToAction("GetShare", new { codeString = codeString });
            }
            return View();

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ShareDownload()
        {
            return RedirectToAction("index", "home");
        }

        [HttpPost]
        [AllowAnonymous]
        public FileStreamResult ShareDownload(string codeString, string password, string recoderString)
        {
            int userID = WebSecurity.CurrentUserId;
            Share share = this._shareService.ProcessDownload(codeString, userID, password);
            IResult result = this._recoderService.ValidateRecoder(recoderString);
            if (share != null && result.Success)
            {
                string absolutePath = Server.MapPath("~/UploadCache/" + share.RealFile.FileName);
                return File(new FileStream(absolutePath, FileMode.Open), "application/octet-stream", Server.UrlEncode(share.FileName + "." + share.FileType));
            }
            return null;
        }
    }
}