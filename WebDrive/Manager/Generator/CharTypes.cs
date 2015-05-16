using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebDrive.Manager.Generator
{
    public class CharTypes
    {
        public const int SYMBOL_CHAR = 1 << 0; //除了字母和数字之外的ASCII在33-126之间的数字
        public const int NUM_CHAR = 1 << 1;
        public const int UPCASE_CHAR = 1 << 2;
        public const int LOWCASE_CHAR = 1 << 3;
        public const int DASH_CHAR = 1 << 4; //下划线和减号
        public const int ALPHA_CHAR = 1 << 5;  //字母
        public const int ALLTYPE_CHAR = (1 << 6) - 1; //上面所有的types

        private static bool IsSymbol(char ch)
        {
            int chInt = (int)ch;
            if ((chInt >= 33 && chInt <= 47) || (chInt >= 58 && chInt <= 64) || (chInt >= 91 && chInt <= 96) || (chInt >= 123 && chInt <= 126))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsNum(char ch)
        {
            int chInt = (int)ch;
            if (chInt >= 48 && chInt <= 57)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsUpcase(char ch)
        {
            int chInt = (int)ch;
            if (chInt >= 65 && chInt <= 90)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsLowcase(char ch)
        {
            int chInt = (int)ch;
            if (chInt >= 97 && chInt <= 122)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsDash(char ch)
        {
            int chInt = (int)ch;
            if (chInt == 95 || chInt == 45)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool IsAlpha(char ch)
        {
            if (IsUpcase(ch) || IsLowcase(ch))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int CharType(char ch)
        {
            int returnType = 0;
            returnType += IsSymbol(ch) ? SYMBOL_CHAR : 0;
            returnType += IsNum(ch) ? NUM_CHAR : 0;
            returnType += IsUpcase(ch) ? UPCASE_CHAR : 0;
            returnType += IsLowcase(ch) ? LOWCASE_CHAR : 0;
            returnType += IsAlpha(ch) ? ALPHA_CHAR : 0;
            returnType += IsDash(ch) ? DASH_CHAR : 0;

            return returnType;
        }

    }
}