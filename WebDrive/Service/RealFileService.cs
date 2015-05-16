using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.DAL.Repository;
using WebDrive.DAL.UnitOfWork;
using WebDrive.Models;
using WebDrive.Service.Interface;

namespace WebDrive.Service
{
    public class RealFileService : IRealFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<RealFile> _repository;

        public RealFileService(IUnitOfWork unitOfWork, IRepository<RealFile> repository)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public IResult AddFile(string name, string type, string size, string MD5)
        {
            IResult result = new Result(false);
            RealFile rf = new RealFile()
            {
                FileName = name,
                FileType = type,
                FileSize = size,
                Available = true,
                CreateDate = DateTime.Now,
                FilePath = "/",
                MD5 = MD5
            };
            try
            {
                this._repository.Insert(rf);
                this._unitOfWork.SaveChange();
                result.Success = true;
                return result;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        public RealFile FindMD5(string MD5)
        {
            List<RealFile> list;
            RealFile rf = null;
            if (MD5 != null)
            {
                rf = this._repository.Get(x => x.MD5 == MD5, null, null).SingleOrDefault();
            }

            return rf;
        }
    }
}