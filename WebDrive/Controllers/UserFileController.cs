using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDrive.Filters;
using WebDrive.Service.Interface;
using WebDrive.Models;
using WebMatrix.WebData;

namespace WebDrive.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class UserFileController : Controller
    {
        private readonly IUserFileService _userFileService;

        public UserFileController(IUserFileService userFileService)
        {
            this._userFileService = userFileService;
        }

        public ActionResult index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FileList()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetUserDir(int parentID = 0)
        {
            int userID = WebSecurity.CurrentUserId;
            List<UserFile> userFiles = this._userFileService.GetDir(parentID, userID);
            UserFile currentUserFile = this._userFileService.GetCurrentDir(parentID);
            var JsonObj = new object[userFiles.Count+1];
            for (int i = 0; i < userFiles.Count; i++)
            {
                JsonObj[i] = new 
                {
                    FileName = userFiles[i].FileName,
                    CreateDate = userFiles[i].CreateDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm"), 
                    FileSize = userFiles[i].Directory?0:int.Parse(userFiles[i].RealFile.FileSize),
                    UserFileID = userFiles[i].UserFileID,
                    ParentFileID = userFiles[i].ParentFileID,
                    Directory = userFiles[i].Directory,
                    FileType = userFiles[i].FileType
                };
            }
            if (currentUserFile != null)
            {
                JsonObj[userFiles.Count] = new
                {
                    currentDirID = currentUserFile.UserFileID,
                    currentParentID = currentUserFile.ParentFileID
                };
            }
            else
            {
                JsonObj[userFiles.Count] = new
                {
                    currentDirID = 0,
                    currentParentID = 0
                };
            }
            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetParentDirID(int currentParentID)
        {
            int ParentDirID = this._userFileService.GetParentDirID(currentParentID);
            return Json(new { ParentDirID = ParentDirID }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CreateNewDir(string dirName, int parentID)
        {
            int userID = WebSecurity.CurrentUserId;
            IResult result = this._userFileService.NewDir(dirName, parentID, userID);
            return Json(new {success = result.Success}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Rename(int fileID, string newName)
        {
            IResult result = this._userFileService.Rename(fileID, newName);
            return Json(new { success = result.Success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Del(List<int> filesToBeDelete)
        {
            IResult result = this._userFileService.Delete(filesToBeDelete);
            return Json(new { success = result.Success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Search(string searchName, int currentDirID, int currentParentID)
        {
            int userID = WebSecurity.CurrentUserId;
            List<UserFile> userFiles = this._userFileService.Search(searchName, userID);
            var JsonObj = new object[userFiles.Count+1];
            for (int i = 0; i < userFiles.Count; ++i)
            {
                JsonObj[i] = new
                {
                    FileName = userFiles[i].FileName,
                    CreateDate = userFiles[i].CreateDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm"),
                    FileSize = userFiles[i].Directory ? 0 : int.Parse(userFiles[i].RealFile.FileSize),
                    UserFileID = userFiles[i].UserFileID,
                    ParentFileID = userFiles[i].ParentFileID,
                    Directory = userFiles[i].Directory,
                    FileType = userFiles[i].FileType
                };
            }
            JsonObj[userFiles.Count] = new
            {
                currentDirID = currentDirID,
                currentParentID = currentParentID
            };
            return Json(JsonObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddExistFile(string fileName, string fileType, int parentID, int realFileID)
        {
            int userID = WebSecurity.CurrentUserId;
            IResult result = this._userFileService.NewFile(fileName, fileType, parentID, realFileID, userID);
            return Json(new { success = result.Success }, JsonRequestBehavior.AllowGet);
        }
    }
}