using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDrive.Manager.Account.QRCode
{
    public class QRCodeGenerator
    {
        private static int RANDOM_QRCODE_LENGTH = 15;
        private static int NORMAL_STATUS = 0;
        private static int WRONG_STATUS = 1;

        public string CodeString { get; set; }

        public QRCodeGenerator()
        {
            CodeString = "";
        }

        public string Refresh()
        {
            CodeString = "";
            RandomString(RANDOM_QRCODE_LENGTH);
            return CodeString;

        }

        private int RandomString(int stringLen)
        {
            if (stringLen <= 0) return WRONG_STATUS;
            char randomChar = ' ';
            Random ran = new Random();
            for (int i = 0; i < stringLen; ++i)
            {
                int randomInt = ran.Next(32, 126);
                randomChar = (char)randomInt;
                CodeString += randomChar;
            }
            return NORMAL_STATUS;
        }
    }
}