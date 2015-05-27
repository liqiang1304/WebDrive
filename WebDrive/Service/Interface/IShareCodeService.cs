using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.Models;

namespace WebDrive.Service.Interface
{
    public interface IShareCodeService
    {
        string CreateCodeByShareID(int shareID);

        ShareCode Refresh(ShareCode shareCode, int expireMinutes);

        ShareCode NewShareCode(int shareID, int expireMinutes);

        string GetCodeStringByCode(string validateString);
    }
}
