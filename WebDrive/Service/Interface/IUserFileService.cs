using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.Models;

namespace WebDrive.Service.Interface
{
    public interface IUserFileService
    {
        List<UserFile> GetDir(int ParentID, int userID);

        UserFile GetCurrentDir(int ID, int userID);

        int GetParentDirID(int CurrentID);

        IResult NewDir(string name, int parentID, int userID);
    }
}
