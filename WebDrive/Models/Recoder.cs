using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebDrive.Models
{
    [Table("Recoder")]
    public class Recoder
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReocderID { get; set; }

        public string RecoderString { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireTime { get; set; }
        public bool Available { get; set; }
    }
}