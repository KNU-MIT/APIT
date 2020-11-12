using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using DatabaseLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayer
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public override DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<Conference> Conferences { get; set; }

        public DbSet<ConferenceParticipant> ConfParticipants { get; set; }
        public DbSet<ConferenceImage> ConfImages { get; set; }
        public DbSet<ConferenceDate> ConfDates { get; set; }

        public DbSet<UserOwnArticlesLinking> UserArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define uncertain relationships and linking tables

            builder.Entity<UserOwnArticlesLinking>()
                .HasKey(x => new {x.UserId, x.ArticleId});

            builder.Entity<UserOwnArticlesLinking>()
                .HasOne(a => a.Article)
                .WithMany(a => a.Authors)
                .HasForeignKey(a => a.ArticleId);


            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "root_admin",
                    NormalizedName = "ROOT_ADMIN"
                }, new IdentityRole
                {
                    Name = "manager",
                    NormalizedName = "MANAGER"
                }
            );


            /*
            var userId = Guid.NewGuid().ToString();
            builder.Entity<User>().HasData(new User()
            {
                Id = userId,
                FirstName = "Big",
                LastName = "Boss",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "test@test.com",
                NormalizedEmail = "TEST@TEST.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>()
                    .HashPassword(null, "admin"),
    
                ScienceDegree = ScienceDegree.First,
                AcademicTitle = AcademicTitle.BestOfTheBest,
                ParticipationForm = ParticipationForm.Admin,
    
                ProfileAddress = "admin-the-best"
            });
    
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = roleId,
                UserId = userId
            });
            */
        }

        public void ClearDatabase()
        {
            Articles.RemoveRange(Articles);
            Users.RemoveRange(Users);
            Topics.RemoveRange(Topics);
            SaveChanges();
        }
    }
}