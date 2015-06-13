using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Models;

namespace WebDrive.Manager.Security
{
    public class AccountSecurity
    {
        private UserProfile _userProfile;

        public AccountSecurity(UserProfile userprofile)
        {
            this._userProfile = userprofile;
        }

        public bool ValidateAll()
        {
            bool returnPara = true;
            returnPara &= ValidateCount();
            returnPara &= ValidateDateTime();

            return returnPara;
        }

        public bool ValidateDateTime()
        {
            DateTime dt = this._userProfile.ExpireLoginDate;
            DateTime noDate = DateTime.Parse("1900-01-01 00:00");
            if (DateTime.Compare(dt, noDate) == 0)
            {
                return true;
            }
            else
            {
                int compare = DateTime.Compare(DateTime.Now, dt);
                return compare <= 0 ? true : false;
            }
        }

        public bool ValidateCount()
        {
            if (this._userProfile.ExpireLoginCounts == 0 || (this._userProfile.ExpireLoginCounts-1 >= this._userProfile.LoginCounts))
            {
                return true;
            }
            return false;
        }
    }
}