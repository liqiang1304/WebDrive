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
    public class RecoderService : IRecoderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Recoder> _repository;

        public RecoderService(IUnitOfWork unitOfWork, IRepository<Recoder> repository)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public Recoder CreateRecoder(int expireMinutes)
        {
            Recoder recoder = new Recoder()
            {
                RecoderString = Guid.NewGuid().ToString(),
                CreateDate = DateTime.Now,
                ExpireTime = DateTime.Now.AddMinutes(expireMinutes),
                Available = true
            };
            this._repository.Insert(recoder);
            this._unitOfWork.SaveChange();
            return recoder;
        }

        public IResult ValidateRecoder(string recoderString)
        {
            IResult result = new Result(false);
            Recoder recoder = null;
            if (recoderString != null)
            {
                recoder = this._repository.Get(x => x.RecoderString == recoderString, null, null).SingleOrDefault();
                if (recoder != null)
                {
                    if (DateTime.Compare(DateTime.Now, recoder.ExpireTime) <= 0)
                    {
                        result.Success = true;
                    }
                    this._repository.Delete(recoder);
                    this._unitOfWork.SaveChange();
                }
            }
            return result;
        }
    }
}