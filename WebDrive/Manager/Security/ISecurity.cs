using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDrive.Manager.Security
{
    public interface ISecurity
    {
        bool validate(object instance);

        bool validateDateTime();

        bool validateCounts();

    }
}
