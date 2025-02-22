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
        
           builder.Entity<User>()
                .HasMany(u => u.TeachingClasses)
                .WithMany(c => c.Teachers)
                .UsingEntity(j => j.ToTable("TeacherClasses"));

            // Veza između učenika i razreda
           builder.Entity<User>()
    .HasOne(u => u.SchoolClass)
    .WithMany(c => c.Students)
    .HasForeignKey(u => u.SchoolClassId)
    .IsRequired(false);  // This ensures SchoolClassId can be nullable

        
       


        






        }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

    
       
    }
}
