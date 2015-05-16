using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Models;
using WebDrive.Service.Interface;
using WebDrive.Manager.Generator;
using ZXing;
using System.Drawing;
using System.IO;
using ZXing.Common;
using ZXing.QrCode.Internal;
using ZXing.QrCode;
using System.Web.Mvc;
using WebDrive.Manager.Account;
using WebDrive.Models;

namespace WebDrive.Manager.QRCode
{
    public class QRCodeManager
    {
        public QRCodeManager()
        {
        }

        public UserProfile ProcessCodeImage(IUserProfileService userProfileService, HttpPostedFileBase file){
            if (file != null)
            {
                try
                {

                    string pic = System.IO.Path.GetFileName(file.FileName);
                    StringGenerator randomName = new StringGenerator();
                    pic = randomName.Refresh(StringGenerator.FILENAME_TYPE, 10) + pic;
                    string path = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/QRCodeCache/"), pic);

                    file.SaveAs(path);

                    IBarcodeReader reader = new BarcodeReader();
                    var barcodeBitmap = (Bitmap)Bitmap.FromFile(path);
                    var result = reader.Decode(barcodeBitmap);
                    barcodeBitmap.Dispose();
                    if (result != null)
                    {
                        string decodeString = result.Text;
                        UserProfile user = userProfileService.GetUserByQRCode(decodeString);
                        System.IO.File.Delete(path);
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine(e);
                }
            }
            return null;
        }

        public MemoryStream GenerateQRCodeImage(string codeString, int height = 300, int width = 300)
        {
            MemoryStream ms = new MemoryStream();
            if (codeString == null || codeString == "") return ms;
            EncodingOptions option = new QrCodeEncodingOptions
            {
                DisableECI = true,
                CharacterSet = "UTF-8",
                Width = width,
                Height = height,
                ErrorCorrection = ErrorCorrectionLevel.H
            };
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = option;
            Bitmap bitmap = writer.Write(codeString);
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Dispose();
            return ms;
        }

        public WebDrive.Models.QRCode GenerateInfo(int userID, DateTime expireDate, string codeString)
        {
            WebDrive.Models.QRCode insertCode = new WebDrive.Models.QRCode()
            {
                UserID = userID,
                CreateDate = DateTime.Now,
                ExpireDate = expireDate,
                CodeString = codeString,
                Available = true
            };
            return insertCode;
        }
    }
}