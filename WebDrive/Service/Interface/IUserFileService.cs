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

        UserFile GetCurrentDir(int ID);

        int GetParentDirID(int CurrentID);

        IResult NewDir(string name, int parentID, int userID);

        IResult NewFile(string name, string type, int parentID, int realFileID, int userID);

        IResult Rename(int FileID, string newName);

        IResult Delete(List<int> filesIDToBeDel);

        List<UserFile> Search(string searchName, int userID);

        UserFile GetByID(int userFileID);

        List<UserFile> GetAllFileByType(string fileType, int userID);
    }
}
