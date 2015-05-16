using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDrive.Manager.Generator
{
    public class StringGenerator
    {
        private const int RANDOM_QRCODE_LENGTH = 15;
        private const int NORMAL_STATUS = 0;
        private const int WRONG_STATUS = 1;

        public const int USERNAME_LEN = 10;
        public const int PASSWORD_LEN = 10;
        public const int NICKNAME_LEN = 7;
        public const int QRCODE_LEN = 15;
        public const int FILENAME_LEN = 10;
        public const int VALIDATE_LEN = 6;

        public const int USERNAME_TYPE = 0;
        public const int PASSWORD_TYPE = 1;
        public const int NICKNAME_TYPE = 2;
        public const int QRCODE_TYPE = 3;
        public const int FILENAME_TYPE = 4;
        public const int VALIDATE_TYPE = 5;

        private static int USERNAME_USE = CharTypes.ALPHA_CHAR + CharTypes.UPCASE_CHAR + CharTypes.LOWCASE_CHAR + CharTypes.NUM_CHAR + CharTypes.DASH_CHAR;
        private static int PASSWORD_USE = CharTypes.ALLTYPE_CHAR;
        private static int NICKNAME_USE = CharTypes.ALPHA_CHAR + CharTypes.UPCASE_CHAR + CharTypes.LOWCASE_CHAR + CharTypes.NUM_CHAR;
        private static int QRCODE_USE = CharTypes.ALLTYPE_CHAR;
        private static int FILENAME_USE = CharTypes.ALPHA_CHAR + CharTypes.UPCASE_CHAR + CharTypes.LOWCASE_CHAR + CharTypes.NUM_CHAR;
        private static int VALIDATE_USE = CharTypes.NUM_CHAR;

        public string CodeString { get; set; }
        public string UsernameString { get; set; }
        public string PasswordString { get; set; }
        public string NicknameString { get; set; }
        public string FilenameString { get; set; }
        public string ValidateString { get; set; }

        private Random ran { get; set; }

        public StringGenerator()
        {
            CodeString = "";
            ran = new Random();
        }

        public char RandomChar()
        {
            int randomInt = ran.Next(32, 126);
            char randomChar = (char)randomInt;
            return randomChar;
        }

        public string Refresh(int GenerateType, int len)
        {
            char tmpCh;
            for (int i = 0; i < len; ++i)
            {
                tmpCh = RandomChar();
                int type = CharTypes.CharType(tmpCh);
                switch (GenerateType)
                {
                    case USERNAME_TYPE:
                        if ((type & USERNAME_USE) == 0) i--;
                        else UsernameString += tmpCh;
                        break;
                    case PASSWORD_TYPE:
                        if ((type & PASSWORD_USE) == 0) i--;
                        else PasswordString += tmpCh;
                        break;
                    case NICKNAME_TYPE:
                        if ((type & NICKNAME_USE) == 0) i--;
                        else NicknameString += tmpCh;
                        break;
                    case QRCODE_TYPE:
                        if ((type & NICKNAME_USE) == 0) i--;
                        else CodeString += tmpCh;
                        break;
                    case FILENAME_TYPE:
                        if ((type & FILENAME_USE) == 0) i--;
                        else FilenameString += tmpCh;
                        break;
                    case VALIDATE_TYPE:
                        if ((type & VALIDATE_USE) == 0) i--;
                        else ValidateString += tmpCh;
                        break;
                    default:
                        return "";
                }
            }
            if (GenerateType == USERNAME_TYPE) return UsernameString;
            else if (GenerateType == PASSWORD_TYPE) return PasswordString;
            else if (GenerateType == NICKNAME_TYPE) return NicknameString;
            else if (GenerateType == QRCODE_TYPE) return CodeString;
            else if (GenerateType == FILENAME_TYPE) return FilenameString;
            else if (GenerateType == VALIDATE_TYPE) return ValidateString;
            else return "";
        }

        public void RefreshAll()
        {
            Refresh(USERNAME_TYPE, USERNAME_LEN);
            Refresh(PASSWORD_TYPE, PASSWORD_LEN);
            Refresh(NICKNAME_TYPE, NICKNAME_LEN);
            Refresh(QRCODE_TYPE, QRCODE_LEN);
            Refresh(FILENAME_TYPE, FILENAME_LEN);
        }

        //refresh username and password
        public void RefreshUser()
        {
            Refresh(USERNAME_TYPE, USERNAME_LEN);
            Refresh(PASSWORD_TYPE, PASSWORD_LEN);
        }
    }
}