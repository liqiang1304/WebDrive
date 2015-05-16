using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Models;

namespace WebDrive.Manager.Security
{
    public class QRCodeSecurity
    {
        private Models.QRCode _qrCode;

        public QRCodeSecurity(Models.QRCode qrCode)
        {
            this._qrCode = qrCode;
        }


    }
}