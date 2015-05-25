using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDrive.Filters;
using WebDrive.Models;
using WebDrive.Service.Interface;
using WebMatrix.WebData;

namespace WebDrive.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class RealFileController : Controller
    {
        private readonly IRealFileService _realFileService;
        private readonly IUserFileService _userFileService;
        private string _MD5;

        public RealFileController(IRealFileService realFileService, IUserFileService userFileService)
        {
            this._realFileService = realFileService;
            this._userFileService = userFileService;
        }

        [HttpGet]
        public ActionResult index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult IsRepeatMD5(string MD5)
        {
            RealFile rf = this._realFileService.FindMD5(MD5);
            if(rf !=null){
                return Json(new { IsRepeat = true, RealFileID = rf.RealFileID }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { IsRepeat = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void PostMD5(string MD5)
        {
            this._MD5 = MD5;
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Upload(HttpPostedFileBase fileData, string MD5, int currentDirID)
        {
            int userID = WebSecurity.CurrentUserId;
            if (fileData != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/UploadCache/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string fileNameWithOutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                    string fileSize = fileData.ContentLength.ToString(); ;

                    fileData.SaveAs(filePath + saveName);
                    IResult result = this._realFileService.AddFile(saveName, fileExtension, fileSize, MD5);
                    if (result.Success)
                    {
                        result = this._userFileService.NewFile(fileNameWithOutExtension, fileExtension, currentDirID, result.ReturnInt, userID);
                    }

                    return Json(new { Success = result.Success, FileName = fileName, SaveName = saveName });
                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {

                return Json(new { Success = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public FileStreamResult DownloadFile(int userFileID)
        {
            int userID = WebSecurity.CurrentUserId;
            UserFile userFile = this._userFileService.GetByID(userFileID);
            if (userFile != null && userFile.UserID == userID)
            {
                string absolutePath = Server.MapPath("~/UploadCache/" + userFile.RealFile.FileName);
                return File(new FileStream(absolutePath, FileMode.Open), "application/octet-stream", Server.UrlEncode(userFile.FileName + "." + userFile.FileType));
            }
            return null;
        }
    }
}