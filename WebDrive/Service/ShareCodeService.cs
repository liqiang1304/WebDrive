using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.UnitOfWork;
using WebDrive.DAL.Repository;
using WebDrive.Models;
using WebDrive.Manager.Generator;

namespace WebDrive.Service
{
    public class ShareCodeService : IShareCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ShareCode> _repository;

        public ShareCodeService(IUnitOfWork unitOfWork, IRepository<ShareCode> repository)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public string CreateCodeByShareID(int shareID)
        {
            ShareCode shareCode = this._repository.Get(x => x.ShareID == shareID, null, null).SingleOrDefault();
            if (shareCode != null)
            {
                shareCode = Refresh(shareCode, 10);
            }
            else
            {
                shareCode = NewShareCode(shareID, 10);
            }
            if (shareCode != null)
            {
                return shareCode.ValidateString;
            }
            else
            {
                return null;
            }
        }

        public ShareCode NewShareCode(int shareID, int expireMinutes)
        {
            StringGenerator sg = new StringGenerator();
            ShareCode shareCode = new ShareCode()
            {
                ShareID = shareID,
                Available = true,
                CreateDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMinutes(expireMinutes),
                ValidateString = sg.Refresh(StringGenerator.VALIDATE_TYPE, StringGenerator.VALIDATE_LEN),
            };
            try
            {
                this._repository.Insert(shareCode);
                this._unitOfWork.SaveChange();
            }
            catch (Exception e)
            {
                return null;
            }
            return shareCode;
        }

        public ShareCode Refresh(ShareCode shareCode, int expireMinutes)
        {
            if (DateTime.Compare(DateTime.Now, shareCode.ExpireDate) >= 0)
            {
                StringGenerator sg = new StringGenerator();
                shareCode.ValidateString = sg.Refresh(StringGenerator.VALIDATE_TYPE, StringGenerator.VALIDATE_LEN);
                shareCode.ExpireDate = DateTime.Now.AddMinutes(expireMinutes);
                try
                {
                    this._repository.Update(shareCode);
                    this._unitOfWork.SaveChange();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return shareCode;
        }

        public string GetCodeStringByCode(string validateString)
        {
            ShareCode shareCode = this._repository.Get(x=>(x.ValidateString == validateString && DateTime.Compare(DateTime.Now, x.ExpireDate)<0), null, null).SingleOrDefault();
            if(shareCode!=null){
                return shareCode.Share.SharedQRCode;
            }
            return null;
        }

    }
}