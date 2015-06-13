using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.UnitOfWork;
using WebDrive.DAL.Repository;
using WebDrive.Models;
using System.Text.RegularExpressions;

namespace WebDrive.Service
{
    public class UserFileService : IUserFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserFile> _repository;

        public UserFileService(IUnitOfWork unitOfWork, IRepository<UserFile> repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public List<UserFile> GetDir(int ParentID, int userID)
        {
            return this._repository.Get(x => x.ParentFileID == ParentID && x.UserID == userID, null, null).ToList();
        }

        public int GetParentDirID(int CurrentParentID)
        {
            if (CurrentParentID == 0) return 0;
            UserFile userFile = this._repository.Get(x => x.UserFileID == CurrentParentID, null, null).SingleOrDefault();
            if (userFile != null)
            {
                return userFile.ParentFileID;
            }
            else
            {
                return 0;
            }
        }

        public UserFile GetCurrentDir(int ID)
        {
            UserFile userFile = this._repository.Get(x => x.UserFileID == ID, null, null).SingleOrDefault();
            return userFile;

        }

        public IResult NewDir(string name, int parentID, int userID)
        {
            IResult result = new Result(false);
            UserFile userFile = new UserFile()
            {
                CreateDate = DateTime.Now,
                Directory = true,
                FileName = name,
                ParentFileID = parentID,
                FileType = "dir",
                UserID = userID,
                RealFileID = 1,
            };
            try
            {
                this._repository.Insert(userFile);
                this._unitOfWork.SaveChange();
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        public IResult Rename(int FileID, string newName)
        {
            IResult result = new Result(false);
            UserFile userFile = this._repository.Get(x => x.UserFileID == FileID, null, null).SingleOrDefault();
            if (userFile == null) return result;
            userFile.FileName = newName;
            try
            {
                this._repository.Update(userFile);
                this._unitOfWork.SaveChange();
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        public IResult NewFile(string name, string type, int parentID, int realFileID, int userID)
        {
            IResult result = new Result(false);
            UserFile userFile = new UserFile()
            {
                FileName = name,
                FileType = type,
                ParentFileID = parentID,
                RealFileID = realFileID,
                UserID = userID,
                Directory = false,
                CreateDate = DateTime.Now,
            };
            try
            {
                this._repository.Insert(userFile);
                this._unitOfWork.SaveChange();
                result.Success = true;
                result.ReturnInt = userFile.UserFileID;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }


        public IResult Delete(List<int> filesIDToBeDel)
        {
            IResult result = new Result(false);
            for (int i = 0; i < filesIDToBeDel.Count; ++i)
            {
                result = DelSubFloder(filesIDToBeDel[i]);
                if (result.Success == false) break;
            }
            try
            {
                this._unitOfWork.SaveChange();
            }
            catch (Exception e)
            {
                result.Success = true;
            }
            return result;
        }

        private IResult DelSubFloder(int fileToBeDel)
        {
            IResult result = new Result(false);
            if(fileToBeDel!=0){
                List<UserFile> userFiles = this._repository.Get(x => x.ParentFileID == fileToBeDel, null, null).ToList();
                for (int i = 0; i < userFiles.Count; ++i)
                {
                    IResult returnResult = DelSubFloder(userFiles[i].UserFileID);
                    if (returnResult.Success == false) return returnResult;
                }
                UserFile userFile = this._repository.Get(x => x.UserFileID == fileToBeDel, null, null).SingleOrDefault();
                if (userFile != null)
                {
                    try
                    {
                        this._repository.Delete(userFile);
                        result.Success = true;
                    }
                    catch(Exception e)
                    {
                        result.Exception = e;
                    }
                }
            }
            return result;
        }

        public List<UserFile> Search(string searchName, int userID)
        {
            List<UserFile> userFiles = this._repository.Get(x => (x.FileName.Contains(searchName) && x.UserID == userID), null, null).ToList();
            return userFiles;
        }

        public List<UserFile> GetAllFileByType(string fileType, int userID)
        {
            var pattern = new string[]{};
            switch (fileType)
            {
                case "Pictures":
                    pattern = new string[] { ".jpg", ".jpeg", ".bmp", ".png", ".tif", ".tiff", ".cdr", ".pcd", ".psd", ".tga" };
                    break;
                case "Documents":
                    pattern = new string[] { ".txt", ".wps", ".doc", ".rtf", "ppt", ".docx", ".pptx", ".html", ".pdf", ".xls", ".xlsx" };
                    break;
                case "Videos":
                    pattern = new string[] { ".avi", ".rmvb", ".rm", ".asf", ".divx", ".mpg", ".mpeg", ".wmv", ".mp4", ".mkv", ".vob", ".flv" };
                    break;
                case "Torrents":
                    pattern = new string[] { ".torrent" };
                    break;
                case "Musics":
                    pattern = new string[] {".aiff", ".ape", ".mp3", ".mp1", ".mp2", ".wma", ".aac", ".ogg", ".wave", ".ape", ".flac"};
                    break;

            }

            List<UserFile> userFiles = this._repository.Get(x => (pattern.Contains(x.FileType) && x.UserID == userID), null, null).ToList();
            return userFiles;
        }

        public UserFile GetByID(int userFileID)
        {
            return this._repository.Get(x => x.UserFileID == userFileID, null, null).SingleOrDefault();
        }
    }
}