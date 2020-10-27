using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBooking.API.Models;
using Newtonsoft.Json;

namespace MyBooking.API.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> //DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePic> TouristRoutePics { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.Entity<TouristRoute>().HasData(new TouristRoute()
             {
                 Id = Guid.NewGuid(),
                 Title="TestTitle",
                 Description="description",
                 OriginalPrice=0,
                 CreateTime =DateTime.UtcNow
             }) ;*/
            var touristRouteJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutesMockData.json");
            IList<TouristRoute> touristRoutes = JsonConvert.DeserializeObject<IList<TouristRoute>>(touristRouteJsonData);
            modelBuilder.Entity<TouristRoute>().HasData(touristRoutes);

            var touristRoutePicJsonData = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/Database/touristRoutePicturesMockData.json");
            IList<TouristRoutePic> touristRoutePics = JsonConvert.DeserializeObject<IList<TouristRoutePic>>(touristRoutePicJsonData);
            modelBuilder.Entity<TouristRoutePic>().HasData(touristRoutePics);

            // Add testing roles data
            // 1.Add user_role foreign key relationship
            modelBuilder.Entity<ApplicationUser>(u =>
            u.HasMany(x => x.UserRoles)
            .WithOne().HasForeignKey(ur => ur.UserId).IsRequired());

            // 2.Add roles
            var adminRoleId = "308660dc-ae51-480f-824d-7dca6714c3e2";
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                });
            // 3.Add users
            var adminUserId = "90184155-dee0-40c9-bb1e-b5ed07afc04e";
            var adminUser = new ApplicationUser()
            {
                Id = adminUserId,
                UserName = "admin@mybooking.com",
                NormalizedUserName = "admin@mybooking.com".ToUpper(),
                Email = "admin@mybooking.com",
                NormalizedEmail = "admin@mybooking.com".ToUpper(),
                TwoFactorEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "123456789",
                PhoneNumberConfirmed = false
            };
            var passwordHasher = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Password1,");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // 4.Add roles to users
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = adminRoleId,
                UserId = adminUserId
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
