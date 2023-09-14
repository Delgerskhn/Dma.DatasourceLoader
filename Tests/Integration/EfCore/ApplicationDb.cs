using Dma.DatasourceLoader.Models;
using Microsoft.EntityFrameworkCore;

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
                .HasMany(r => r.NestedCollection)
                .WithOne(r => r.Owner)
                .HasForeignKey(r => r.OwnerId);

            // modelBuilder.Entity<SampleData>()
            //     .HasOne(r => r.NestedData)
            //     .WithOne(r => r.Parent)
            //     .HasForeignKey<SampleNestedData>(r => r.ParentId);

            // modelBuilder.Entity<SampleNestedData>()
            //     .HasOne(r => r.DeepNestedData)
            //     .WithOne(r => r.Owner)
            //     .HasForeignKey<DeepNestedData>(r => r.OwnerId);



        }
    }
}
