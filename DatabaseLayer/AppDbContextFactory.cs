using System;
using System.IO;
using DatabaseLayer.ConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace DatabaseLayer
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private const string AppConfigPath = "../Apit/appsettings.json";
        
        public AppDbContext CreateDbContext(string[] args)
        {
            var configData = File.Exists(AppConfigPath)
                ? JsonSerializer.Deserialize<ProjectConfig>(AppConfigPath)
                : throw new FileNotFoundException("Config file not found: " + AppConfigPath);
            
            Console.WriteLine(" Using database connection is: " + configData.SelectedConnectionString);
            
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(configData.ConnectionStrings[configData.SelectedConnectionString]);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}