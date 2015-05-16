using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Manager.Generator;

namespace WebDrive.Manager.Account
{
    public class UserInfoManager
    {
        public object registerOjb { get; set; }
        public object AnonymousOjb { get; set; }
        public StringGenerator user { get; set; }

        public UserInfoManager()
        {
            user = new StringGenerator();
        }

        public object Register(string email, string nickName)
        {
            user.Refresh(StringGenerator.QRCODE_TYPE, StringGenerator.QRCODE_LEN);
            registerOjb = new
            {
                Email = email,
                NickName = nickName,
                RegisterDate = DateTime.Now,
                Available = 1,
                LoginCounts = 0,
                ExpireLoginDate = DateTime.Parse("1900-01-01 00:00"),
                ExpireLoginCounts = 0
            };
            return registerOjb;
        }

        public object AnonymousRegister(DateTime expireLoginDate, int expireLoginCounts)
        {
            user.RefreshAll();
            AnonymousOjb = new
            {
                NickName = user.NicknameString,
                RegisterDate = DateTime.Now,
                ExpireLoginDate = expireLoginDate,
                Available = 1,
                LoginCounts = 0,
                ExpireLoginCounts = expireLoginCounts
            };
            return AnonymousOjb;
        }
    }
}