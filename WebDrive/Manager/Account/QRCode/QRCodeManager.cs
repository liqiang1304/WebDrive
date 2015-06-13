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
using System.Drawing.Imaging;

namespace WebDrive.Manager.QRCode
{
    public class QRCodeManager
    {
        public QRCodeManager()
        {
        }

        /*
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
         */
        public UserProfile ProcessCodeImage(IUserProfileService userProfileService, HttpPostedFileBase file)
        {
            string decodeString = GetCodeString(file);
            UserProfile user = userProfileService.GetUserByQRCode(decodeString);
            return user;
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

        public string GetCodeString(HttpPostedFileBase file)
        {
            if (file != null)
            {
                try
                {

                    string pic = System.IO.Path.GetFileName(file.FileName);
                    StringGenerator randomName = new StringGenerator();
                    pic = randomName.Refresh(StringGenerator.FILENAME_TYPE, 10) + pic;
                    string path = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/QRCodeCache/"), pic);
                    string path1 = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/QRCodeCache/Gray"), pic);
                    file.SaveAs(path);

                    IBarcodeReader reader = new BarcodeReader() { AutoRotate = true, TryHarder = true };
                    var barcodeBitmap = (Bitmap)Bitmap.FromFile(path);
                    var grayBitmap = Gray(barcodeBitmap);
                    var result = reader.Decode(grayBitmap);
                    barcodeBitmap.Dispose();

                    if (result != null)
                    {
                        string decodeString = result.Text;
                        System.IO.File.Delete(path);
                        return decodeString;
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

        public Bitmap Gray(Bitmap b)
        {
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                int nOffset = stride - b.Width * 3;
                byte red, green, blue;
                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < b.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];
                        p[0] = p[1] = p[2] = (byte)(.3333 * red + .3333 * green + .3333 * blue);
                        p += 3;
                    }
                    p += nOffset;
                }
            }
            b.UnlockBits(bmData);
            return b;
        }  
    }
}