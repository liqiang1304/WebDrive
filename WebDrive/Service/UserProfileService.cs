using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.Models;
using WebDrive.DAL.Repository;
using WebDrive.DAL.UnitOfWork;
using WebDrive.Manager.QRCode;
using WebDrive.Manager.Security;
using System.Web.Security;

namespace WebDrive.Service
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserProfile> _repository;

        public UserProfileService(IUnitOfWork unitOfWork, IRepository<UserProfile> repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
        }

        public UserProfile GetByUsername(string username)
        {
            return this._repository.Get(x => x.UserName == username, null, null).SingleOrDefault();
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return this._repository.GetAll().ToList();
        }

        public UserProfile GetUserByQRCode(string codeString)
        {
            return this._repository.Get(x => x.QRCode.CodeString == codeString, null, null).SingleOrDefault();
        }

        public IResult AddLoginCount(string username)
        {
            IResult result = new Result(false);
            try
            {
                UserProfile user = GetByUsername(username);
                user.LoginCounts++;
                this._repository.Update(user);
                this._unitOfWork.SaveChange();
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }
            return result;
        }

        public IResult QRCodeLogin(HttpPostedFileBase file)
        {
            IResult result = new Result(false);
            if (file == null) return result;
            QRCodeManager qrCodeManager = new QRCodeManager();
            UserProfile user = qrCodeManager.ProcessCodeImage(this, file);
            if (user == null) return result;

            AccountSecurity account = new AccountSecurity(user);
            if (account.ValidateAll())
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                this.AddLoginCount(user.UserName);
                result.Success = true;
                return result;
            }
            return result;
        }

    }
}