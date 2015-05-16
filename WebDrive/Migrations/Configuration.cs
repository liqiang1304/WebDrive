namespace WebDrive.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebDrive.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebDrive.DAL.Context.WebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebDrive.DAL.Context.WebContext context)
        {
            var userProfile = new List<UserProfile>
            {
                new UserProfile{UserName = "user1", Email = "user1@wd.com", NickName="userNickname1", RegisterDate = DateTime.Parse("2015-01-21"), ExpireLoginDate = DateTime.Parse("2015-07-20"), Available = true, LoginCounts = 0, ExpireLoginCounts = 100},
                new UserProfile{UserName = "test2", Email = "test2@wd.com", NickName="testNickname1", RegisterDate = DateTime.Parse("2015-02-4"), ExpireLoginDate = DateTime.Parse("2015-06-10"), Available = true, LoginCounts = 0, ExpireLoginCounts = 130},
                new UserProfile{UserName = "admin", Email = "admin@wd.com", NickName="administrator", RegisterDate = DateTime.Parse("2005-01-01"), ExpireLoginDate = DateTime.Parse("2020-12-31"), Available = true, LoginCounts = 0, ExpireLoginCounts = 0},
                new UserProfile{UserName = "sqh14001", Email = "sqh14001@163.com", NickName="SuQihua", RegisterDate = DateTime.Parse("2014-08-14"), ExpireLoginDate = DateTime.Parse("2015-04-10"), Available = true, LoginCounts = 0, ExpireLoginCounts = 120},
            };
            userProfile.ForEach(s => context.UserProfiles.AddOrUpdate(p => p.UserName, s));
            context.SaveChanges();

            var qrCodes = new List<QRCode>
            {
                new QRCode{
                    UserID = userProfile.Single(s => s.UserName == "user1").UserID,
                    CodeString = "NB(8h(*#Yncfh8923h",
                    CreateDate = DateTime.Parse("2015-01-21"),
                    ExpireDate = DateTime.Parse("2015-07-20"),
                    Available = true,
                },
                new QRCode{
                    UserID = userProfile.Single(s => s.UserName == "test2").UserID,
                    CodeString = "v3IjvfN4t9nJr3ffv(",
                    CreateDate = DateTime.Parse("2015-03-21"),
                    ExpireDate = DateTime.Parse("2015-05-25"),
                    Available = true,
                },
                new QRCode{
                    UserID = userProfile.Single(s => s.UserName == "sqh14001").UserID,
                    CodeString = "MV9439)jmv9fj43)(mk",
                    CreateDate = DateTime.Parse("2015-01-01"),
                    ExpireDate = DateTime.Parse("2015-05-31"),
                    Available = true,
                }
            };
            qrCodes.ForEach(s => context.QRCodes.AddOrUpdate(p => p.UserID, s));
            context.SaveChanges();

            var validationCode = new List<ValidationCode>
            {
                new ValidationCode{
                    UserID = userProfile.Single(s => s.UserName == "user1").UserID,
                    CreateDate = DateTime.Parse("2015-05-13"),
                    Available = true,
                    ValidateString = "483087"
                },
                new ValidationCode{
                    UserID = userProfile.Single(s => s.UserName == "test2").UserID,
                    CreateDate = DateTime.Parse("2015-05-13"),
                    Available = true,
                    ValidateString = "357823"
                }
            };
        }
    }
}
