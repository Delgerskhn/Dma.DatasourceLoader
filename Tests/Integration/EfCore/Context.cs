using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Tests.Integration.EfCore
{

    public static class Context
    {
        public static DbContextOptions<ApplicationDb> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<ApplicationDb>()
                .UseInMemoryDatabase(databaseName)
            .Options;
        }

        //public static ApplicationDb GetRealContext()
        //{
        //    return new ApplicationDb(
        //                     new DbContextOptionsBuilder<ApplicationDb>()
        //                         .UseNpgsql("Server=localhost; Database=centric_dev; Port=5432; User Id=postgres;Password=Pass1234!")
        //    .Options);
        //}

        public static ApplicationDb GetContext(string databaseName, bool reset = true)
        {
            var context = new ApplicationDb(GetOptions(databaseName));

            if (reset)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }
    }
}
