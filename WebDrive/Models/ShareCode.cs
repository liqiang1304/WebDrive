using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebDrive.Models
{
    [Table("ShareCode")]
    public class ShareCode
    {
        [Key]
        [ForeignKey("Share")]
        public int ShareID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool Available { get; set; }
        public string ValidateString { get; set; }

        public virtual Share Share { get; set; }
    }
}