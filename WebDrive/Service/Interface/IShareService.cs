using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.Models;
using WebDrive.ViewModels;

namespace WebDrive.Service.Interface
{
    public interface IShareService
    {
        Share CreateShare(ShareModels shareModel, int realFileID, int userID, string fileName, string fileType);

        Share GetByCodeString(string codeString);

        IResult ValidateShare(string codeString, int userID, string password);

        Share ProcessDownload(string codeString, int userID, string password);
    }
}
