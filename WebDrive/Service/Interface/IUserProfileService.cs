using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebDrive.Models;

namespace WebDrive.Service.Interface
{
    public interface IUserProfileService
    {
        UserProfile GetByUsername(string username);

        IEnumerable<UserProfile> GetAll();

        UserProfile GetUserByQRCode(string codeString);

        IResult AddLoginCount(string username);

        IResult QRCodeLogin(HttpPostedFileBase file);
    }
}
