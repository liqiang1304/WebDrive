using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.Models;

namespace WebDrive.Service.Interface
{
    public interface IRealFileService
    {
        IResult AddFile(string name, string type, string size, string MD5);

        RealFile FindMD5(string MD5);
    }
}
