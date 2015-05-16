using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebDrive.Models
{
    [Table("ValidationCode")]
    public class ValidationCode
    {
        [Key]
        [ForeignKey("UserProfile")]
        public int UserID { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Available { get; set; }
        public string ValidateString { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}