using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDrive.Filters;
using WebDrive.Models;
using WebDrive.Service.Interface;

namespace WebDrive.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class RealFileController : Controller
    {
        private readonly IRealFileService _realFileService;
        private string _MD5;

        public RealFileController(IRealFileService realFileService)
        {
            this._realFileService = realFileService;
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
                return Json(new { IsRepeat = true }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Upload(HttpPostedFileBase fileData, string MD5)
        {
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
                    string saveName = Guid.NewGuid().ToString() + fileExtension; // 保存文件名称
                    string fileSize = fileData.ContentLength.ToString(); ;

                    fileData.SaveAs(filePath + saveName);
                    this._realFileService.AddFile(saveName, fileExtension, fileSize, MD5);

                    return Json(new { Success = true, FileName = fileName, SaveName = saveName });
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
    }
}