using Asp.netApp.Areas.Admin.Models.DataModel;
using Microsoft.EntityFrameworkCore;

namespace Asp.netApp.Areas.Admin.Data
{
    public class DatabaseSeeder
    {
        public static void SeedRolesAndAccounts(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    AccountId = "1",
                    Username = "nam",
                    Password = "e10adc3949ba59abbe56e057f20f883e", // '123456' hashed
                    Fullname = "admin",
                    FirstName = "nam",
                    LastName = "van",
                    Email = "admin@example.com", // Provide an email value
                    Active = true,
                    RoleId = 1
                },
                new Account
                {
                    AccountId = "2",
                    Username = "khoi",
                    Password = "e10adc3949ba59abbe56e057f20f883e", // '123456' hashed
                    Fullname = "khoi",
                    FirstName = "nam",
                    LastName = "van",
                    Email = "khoi@gmail.com",
                    Active = true,
                    RoleId = 2
                },
                new Account
                {
                    AccountId = "3",
                    Username = "viet",
                    Password = "e10adc3949ba59abbe56e057f20f883e", // '123456' hashed
                    Fullname = "viet",
                    FirstName = "nam",
                    LastName = "van",
                    Email = "viet@gmail.com",
                    Active = true,
                    RoleId = 3
                },
                new Account
                {
                    AccountId = "4",
                    Username = "manh",
                    Password = "e10adc3949ba59abbe56e057f20f883e", // '123456' hashed
                    Fullname = "manh",
                    FirstName = "nam",
                    LastName = "van",
                    Email = "manh@gmail.com",
                    Active = true,
                    RoleId = 4
                }
            );
        }
    }
}
