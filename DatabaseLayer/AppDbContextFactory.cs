using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DatabaseLayer
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(
                // aspnet-Apit-C2FA6515-42E2-4E38-BBCB-5CEB88F889AD
                // "Server=(localdb)\\mssqllocaldb;Database=testdb_6.27;Trusted_Connection=True;MultipleActiveResultSets=true");
            "Server=localhost,1433;Database=testdb_12;User=sa;Password=reallyStrongPwd123;MultipleActiveResultSets=true");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}