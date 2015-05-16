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
using WebMatrix.WebData;
using WebDrive.Service.Interface;
using WebDrive.Filters;

namespace WebDrive.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class ValidationCodeController : Controller
    {
        private readonly IValidationCodeService _validationCodeService;
        private readonly IUserProfileService _userProfileService;

        public ValidationCodeController(IValidationCodeService validationCodeService, IUserProfileService userProfileService)
        {
            this._validationCodeService = validationCodeService;
            this._userProfileService = userProfileService;
        }

        [HttpGet]
        public ActionResult _CodeDisplay()
        {
            IResult result = this._validationCodeService.RefreshByUser(this._userProfileService);
            if (result.Success)
            {
                ViewBag.validationString = result.Message;
            }
            return View();
        }

        [HttpGet]
        public string RefreshCode()
        {
            IResult result = this._validationCodeService.RefreshByUser(this._userProfileService);
            return result.Message;
        }
        
    }
}