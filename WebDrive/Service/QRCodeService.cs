using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.UnitOfWork;
using WebDrive.DAL.Repository;
using WebDrive.Models;

namespace WebDrive.Service
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<QRCode> _repositroy;

        public QRCodeService(IUnitOfWork unitOfWork, IRepository<QRCode> repository)
        {
            this._unitOfWork = unitOfWork;
            this._repositroy = repository;
        }

        public IResult Generate(QRCode instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            IResult result = new Result(false);

            try
            {
                this._repositroy.Insert(instance);
                this._unitOfWork.SaveChange();
                result.Success = true;
            }
            catch (Exception e)
            {
                result.Exception = e;
            }

            return result;
        }
    }
}