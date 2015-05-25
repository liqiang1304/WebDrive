using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebDrive.Service.Interface;
using WebDrive.DAL.Repository;
using WebDrive.DAL.UnitOfWork;
using WebDrive.Models;

namespace WebDrive.Service
{
    public class ShareService : IShareService
    {
        private readonly IRepository<Share> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ShareService(IRepository<Share> repository, IUnitOfWork unitOfWork)
        {
            this._repository = repository;
            this._unitOfWork = unitOfWork;
        }
    }
}