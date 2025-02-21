using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StoreContext : IdentityDbContext<User>
    {
        public StoreContext()
        {
        }

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed data for roles
            builder.Entity<IdentityRole>()
                .HasData(
                    new IdentityRole { Name = "Member", NormalizedName = "MEMBER" },
                    new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" }
                );

            // Configure relationships
        
          
        
       


        builder.Entity<Request>()
    .HasOne(r => r.RequestType)
    .WithMany()
    .HasForeignKey(r => r.RequestTypeID)
    .OnDelete(DeleteBehavior.Cascade);


builder.Entity<RequestSubtype>()
    .HasOne(rs => rs.RequestType)
    .WithMany(rt => rt.RequestSubTypeList)
    .HasForeignKey(rs => rs.RequestTypeID)
    .OnDelete(DeleteBehavior.Cascade);



        }

        public DbSet<Appointment> Appointments { get; set; }
      
        public DbSet<Municipality> Municipalities { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestSubtype> RequestSubtypes { get; set; }
       
    }
}
