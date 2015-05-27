using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using System.Web.Configuration;
using WebDrive.DAL.DbContextFactory;
using WebDrive.DAL.Repository;
using WebDrive.Models;
using WebDrive.Service;
using WebDrive.Service.Interface;
using WebDrive.DAL.UnitOfWork;

namespace WebDrive
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
        string connectionString = WebConfigurationManager.ConnectionStrings["WebContext"].ConnectionString;

        container.RegisterType<IDbContextFactory, DbContextFactory>(
        new PerRequestLifetimeManager(),
        new InjectionConstructor(connectionString));

        //Repository
        container.RegisterType<IRepository<UserProfile>, GenericRepository<UserProfile>>();
        container.RegisterType<IRepository<QRCode>, GenericRepository<QRCode>>();
        container.RegisterType<IRepository<ValidationCode>, GenericRepository<ValidationCode>>();
        container.RegisterType<IRepository<RealFile>, GenericRepository<RealFile>>();
        container.RegisterType<IRepository<UserFile>, GenericRepository<UserFile>>();
        container.RegisterType<IRepository<Share>, GenericRepository<Share>>();
        container.RegisterType<IRepository<Recoder>, GenericRepository<Recoder>>();

        //Service
        container.RegisterType<IUserProfileService, UserProfileService>();
        container.RegisterType<IQRCodeService, QRCodeService>();
        container.RegisterType<IValidationCodeService, ValidationCodeService>();
        container.RegisterType<IRealFileService, RealFileService>();
        container.RegisterType<IUserFileService, UserFileService>();
        container.RegisterType<IShareService, ShareService>();
        container.RegisterType<IRecoderService, RecoderService>();

        //UnitOfWork
        container.RegisterType<IUnitOfWork, UnitOfWork>();
    }
  }
}