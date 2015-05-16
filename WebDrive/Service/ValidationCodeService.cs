using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.UnitOfWork;
using WebDrive.DAL.Repository;
using WebDrive.Models;
using WebDrive.Service.Interface;
using WebMatrix.WebData;
using WebDrive.Manager.Generator;
using WebDrive.Manager.CoAccess;
using WebDrive.Filters;
using System.Web.Security;
namespace WebDrive.Service
{
    [InitializeSimpleMembership]
    public class ValidationCodeService : IValidationCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<ValidationCode> _repository;
        private ValidationCodeManager _validationManager;

        public ValidationCodeService(IUnitOfWork unitOfWork, IRepository<ValidationCode> repository)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._validationManager = new ValidationCodeManager();
        }

        public IResult Create()
        {
            ValidationCode instance = this._validationManager.CreatedByUserID(WebSecurity.CurrentUserId);
            IResult result = new Result(false);
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                this._repository.Insert(instance);
                this._unitOfWork.SaveChange();
                result.Success = true;
                result.Message = instance.ValidateString;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }

        public IResult RefreshByUser(IUserProfileService userProfileService)
        {
            IResult result = new Result(false);
            if (IsExistsUser())
            {
                StringGenerator sg = new StringGenerator();
                UserProfile user = userProfileService.GetByUsername(WebSecurity.CurrentUserName);
                ValidationCodeManager vc = new ValidationCodeManager(user.ValidationCodes);
                user.ValidationCodes = vc.Refresh();
                this._repository.Update(user.ValidationCodes);
                this._unitOfWork.SaveChange();
                result.Success = true;
                result.Message = user.ValidationCodes.ValidateString;
            }
            else
            {
                IResult createResult = Create();
                if (createResult.Success)
                {
                    result.Message = createResult.Message;
                    result.Success = true;
                }
            }
            return result;
        }

        public bool IsExistsUser()
        {
            ValidationCode validationCode = this._repository.Get(x => x.UserID == WebSecurity.CurrentUserId, null, null).SingleOrDefault();
            if (validationCode == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public UserProfile FindByCodeString(string code)
        {
            DateTime latestCreateTime = DateTime.Now.AddSeconds(-30);
            List<ValidationCode> validations = this._repository.Get(x => (x.ValidateString == code) && (DateTime.Compare(x.CreateDate, latestCreateTime)>=0), null, null).ToList();
            if (validations.Count() != 1)
            {
                return null;
            }
            else
            {
                ValidationCode vc = validations.Single();
                return vc.UserProfile;
            }
        }

        public IResult Disable(ValidationCode instance)
        {
            IResult result = new Result(false);
            if (instance != null)
            {
                ValidationCodeManager vc = new ValidationCodeManager(instance);
                instance = vc.Disable();
                this._repository.Update(instance);
                this._unitOfWork.SaveChange();
                result.Success = true;
                return result;
            }
            return result;
        }

        public IResult ValidationCodeLogin(string code)
        {
            IResult result = new Result(false);
            UserProfile user = FindByCodeString(code);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, false);
                Disable(user.ValidationCodes);
                result.Success = true;
            }
            return result;
        }
    }
}