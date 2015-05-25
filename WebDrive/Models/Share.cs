using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebDrive.Models;

namespace WebDrive.Models
{
    [Table("Share")]
    public class Share
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ShareID { get; set; }

        public int UserID { get; set; }
        public int RealFileID { get; set; }

        public int SharedType { get; set; }
        public DateTime SharedDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string Password { get; set; }
        public int DownloadCounts { get; set; }
        public int ExpireCounts { get; set; }
        public bool Private { get; set; }
        public string SharedQRCode { get; set; }

        public virtual UserFile UserFile { get; set; }
        public virtual RealFile RealFile { get; set; }
    }
}