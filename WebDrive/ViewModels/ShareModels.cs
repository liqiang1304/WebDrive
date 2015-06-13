using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebDrive.ViewModels
{
    public class ShareModels
    {
        public static int PASSWORDNEED  = 1 << 0;
        public static int DATENEED      = 1 << 1;
        public static int DOWNLOADNEED  = 1 << 2;
        public static int PRIVATENEED   = 1 << 3;
        public static int QRCODENEED    = 1 << 4;

        public int userFileID { get; set; }

        [Display(Name = "Sharing type")]
        public int SharedType { get; set; }

        [Display(Name = "Share expire date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpirenDate { get; set; }

        [Display(Name = "Share expire time")]
        [DataType(DataType.Time)]
        public DateTime ExpireTime { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Text)]
        public string Password { get; set; }

        [Display(Name = "Share expire download counts")]
        [DataType(DataType.Text)]
        public int ExpireCounts { get; set; }

        [Display(Name = "Is this share private")]
        public bool Private { get; set; }

        public bool DateLimit { get; set; }
        public bool DownloadLimit { get; set; }
        public bool PasswordNeed { get; set; }
        public bool QRCode { get; set; }

        public DateTime ExpireLoginDateTime
        {
            get
            {
                return ExpirenDate.Add(new TimeSpan(ExpireTime.Hour, ExpireTime.Minute, ExpireTime.Second));
            }
        }
    }
}