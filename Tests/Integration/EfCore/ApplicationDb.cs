using Microsoft.EntityFrameworkCore;
using Tests.DatasourceLoader;

namespace Tests.Integration.EfCore
{
    public class ApplicationDb : DbContext
    {
        public ApplicationDb(DbContextOptions<ApplicationDb> options)
           : base(options)
        {
        }

        public virtual DbSet<SampleData> SampleDatas { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SampleData>()
                .Ignore(r => r.StrCollection)
                .Ignore(r=>r.DateCollection)
                .Ignore(r=>r.NumericCollection);

            modelBuilder.Entity<SampleData>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<SampleNestedData>()
                .HasKey(r => r.Id);
        }
    }
}
