using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.Repository;
using WebDrive.DAL.UnitOfWork;
using WebDrive.Models;
using WebDrive.ViewModels;

namespace WebDrive.Service
{
    public class ShareService : IShareService
    {
        private readonly IRepository<Share> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ShareService(IRepository<Share> repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }

        public Share CreateShare(ShareModels shareModel, int realFileID, int userID, string fileName, string fileType)
        {
            int authorization = 0;
            authorization += shareModel.PasswordNeed? ShareModels.PASSWORDNEED : 0;
            authorization += shareModel.DateLimit ? ShareModels.DATENEED : 0;
            authorization += shareModel.DownloadLimit ? ShareModels.DOWNLOADNEED : 0;
            authorization += shareModel.Private ? ShareModels.PRIVATENEED : 0;
            authorization += shareModel.QRCode ? ShareModels.QRCODENEED : 0;

            string qrCode = Guid.NewGuid().ToString();

            Share share = new Share()
            {
                UserID = userID,
                RealFileID = realFileID,
                SharedType = authorization,
                SharedDate = DateTime.Now,
                ExpireDate = shareModel.DateLimit ? shareModel.ExpireLoginDateTime : DateTime.Now,
                Password = shareModel.PasswordNeed ? shareModel.Password : "0",
                DownloadCounts = 0,
                ExpireCounts = shareModel.DownloadLimit? shareModel.ExpireCounts : 0,
                SharedQRCode = qrCode,
                Private = shareModel.Private,
                FileName = fileName,
                FileType = fileType
            };

            this._repository.Insert(share);
            this._unitOfWork.SaveChange();

            return share;
        }

        public Share GetByCodeString(string codeString)
        {
            Share share = this._repository.Get(x => x.SharedQRCode == codeString, null, null).SingleOrDefault();
            return share;
        }

        public IResult ValidateShare(string codeString, int userID, string password)
        {
            IResult result = new Result(true);
            Share share = GetByCodeString(codeString);
            if (share != null)
            {
                if ((share.SharedType & ShareModels.PRIVATENEED) > 0)
                {
                    if (userID != share.UserID) result.Success = false;
                }
                if ((share.SharedType & ShareModels.DATENEED) > 0)
                {
                    if (DateTime.Compare(DateTime.Now, share.ExpireDate) > 0) result.Success = false;
                }
                if ((share.SharedType & ShareModels.DOWNLOADNEED) > 0)
                {
                    if (share.DownloadCounts > share.ExpireCounts) result.Success = false;
                }
                if ((share.SharedType & ShareModels.PASSWORDNEED) > 0)
                {
                    if (share.Password != password) result.Success = false;
                }
            }
            else
            {
                result.Success = false;
            }
            return result;
        }

        public Share ProcessDownload(string codeString, int userID, string password)
        {
            Share share = null;
            IResult result = ValidateShare(codeString, userID, password);
            if (result.Success)
            {
                share = GetByCodeString(codeString);
                share.DownloadCounts++;
                try
                {
                    this._repository.Update(share);
                    this._unitOfWork.SaveChange();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            return share;
        }
    }
}