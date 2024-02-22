#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WSHAppDB.Model.Entities;

namespace WSHAppDB.Model
{
    //Az adatbázis felépítése
    //Add-Migration WSHDB_v1.0
    public partial class WSHDBContext : DbContext
    {
        public WSHDBContext()
        {
        }

        public WSHDBContext(DbContextOptions<WSHDBContext> options)
            : base(options)
        {
        }


        public static async Task MigrAsync(IServiceProvider services)
        {
            using (var serviceScope = services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<WSHDBContext>();
                var migr = await dbContext.Database.GetPendingMigrationsAsync();
                if (migr.Any())
                {
                    var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<WSHDBContext>>();
                    foreach (var item in migr)
                    {
                        logger.LogInformation("MigrateAsync: {0}", item);
                    }
                    await dbContext.Database.MigrateAsync();
                }
            }
        }

        public virtual DbSet<WSHTransaction> WSHTransaction { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WSHTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId);


            });

            OnModelCreatingPartial(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //ez csak az Update-Database esetén kell optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WSHDB;Trusted_Connection=True;MultipleActiveResultSets=true");
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=WSHDB;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}