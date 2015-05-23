using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebDrive.Models;

namespace WebDrive.Models
{
    [Table("UserFile")]
    public class UserFile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserFileID { get; set; }

        public int RealFileID { get; set; }
        public int UserID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public int ParentFileID { get; set; }
        public bool Directory { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual RealFile RealFile { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}