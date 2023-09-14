using Microsoft.EntityFrameworkCore;

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

        public static ApplicationDb GetRealContext(bool reset = true)
        {
            var context = new ApplicationDb(
                             new DbContextOptionsBuilder<ApplicationDb>()
                                 .UseSqlite("Filename=Test.db")
            .Options);

            if (reset)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return context;
        }

        public static ApplicationDb GetInMemoryContext(string databaseName, bool reset = true)
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
