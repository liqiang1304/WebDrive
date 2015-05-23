using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebDrive.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [StringLength(30, MinimumLength=4)]
        public string UserName { get; set; }

        public string Email { get; set; }
        public string NickName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; }
        [DataType(DataType.DateTime), Display(Name = "Expire Login Time"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime ExpireLoginDate { get; set; }
        public bool Available { get; set; }
        public int LoginCounts { get; set; }
        public int ExpireLoginCounts { get; set; }

        public virtual QRCode QRCode { get; set; }
        public virtual ValidationCode ValidationCodes { get; set; }
        public virtual ICollection<UserFile> UserFiles { get; set; }
    }
}