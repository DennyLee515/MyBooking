using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBooking.API.Models;
using Newtonsoft.Json;

namespace MyBooking.API.Database
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<TouristRoute> TouristRoutes { get; set; }
        public DbSet<TouristRoutePic> TouristRoutePics { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
