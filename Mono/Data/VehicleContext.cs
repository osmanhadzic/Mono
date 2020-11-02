using Microsoft.EntityFrameworkCore;
using Mono.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Mono.Data
{
    public class VehicleContext : DbContext
    {
        public VehicleContext(DbContextOptions<VehicleContext> options):base(options)
        {

        }
        public DbSet<VehicleMake> vehicleMakes { get; set; }
        public DbSet<VehicleModel> vehicleModels { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleMake>().ToTable("vehicleMakes");
            modelBuilder.Entity<VehicleModel>().ToTable("vehicleModels");
        }
    }
}
