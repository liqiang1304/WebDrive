using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.Service.Interface;
using WebDrive.Models;

namespace WebDrive.Service.Interface
{
    public interface IValidationCodeService
    {
        IResult Create();

        IResult RefreshByUser(IUserProfileService userProfileService);

        bool IsExistsUser();

        UserProfile FindByCodeString(string code);

        IResult Disable(ValidationCode instance);

        IResult ValidationCodeLogin(string code);
    }
}
