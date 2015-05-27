using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDrive.Models;

namespace WebDrive.Service.Interface
{
    public interface IRecoderService
    {
        Recoder CreateRecoder(int expireMinutes);

        IResult ValidateRecoder(string recoderString);
    }
}
