using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Models;
using WebDrive.Manager.Generator;
using WebDrive.Service.Interface;

namespace WebDrive.Manager.CoAccess
{
    public class ValidationCodeManager
    {
        public ValidationCode _validationCode;
        private StringGenerator sg;
        public ValidationCodeManager()
        {
            sg = new StringGenerator();
            this._validationCode = new ValidationCode();
        }

        public ValidationCodeManager(ValidationCode validationCode)
        {
            sg = new StringGenerator();
            this._validationCode = validationCode;
        }

        public ValidationCode CreatedByUserID(int userID)
        {
            this._validationCode.UserID = userID;
            this._validationCode.CreateDate = DateTime.Now;
            this._validationCode.Available = true;
            this._validationCode.ValidateString = sg.Refresh(StringGenerator.VALIDATE_TYPE, StringGenerator.VALIDATE_LEN);

            return this._validationCode;
        }

        public ValidationCode Refresh()
        {
            this._validationCode.ValidateString = sg.Refresh(StringGenerator.VALIDATE_TYPE, StringGenerator.VALIDATE_LEN);
            this._validationCode.CreateDate = DateTime.Now;
            this._validationCode.Available = true;
            return this._validationCode;
        }

        public ValidationCode Disable()
        {
            this._validationCode.Available = false;
            return this._validationCode;
        }
    }
}