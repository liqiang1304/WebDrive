using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebDrive.Models
{
    [Table("RealFile")]
    public class RealFile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int RealFileID { get; set; }

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Available { get; set; }
        public string MD5 { get; set; }

        public virtual ICollection<UserFile> UserFiles { get; set; }
    }
}