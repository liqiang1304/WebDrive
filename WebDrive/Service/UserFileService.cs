using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.UnitOfWork;
using WebDrive.DAL.Repository;
using WebDrive.Models;

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

        public int GetParentDirID(int CurrentParentID, int userID)
        {
            if (CurrentParentID == 0) return 0;
            UserFile userFile = this._repository.Get(x => x.UserFileID == CurrentParentID && x.UserID == userID, null, null).SingleOrDefault();
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
    }
}