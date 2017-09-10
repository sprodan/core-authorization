using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Authorization> Authorizations { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Employee> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>()
                .HasMany(r => r.Modules);

            builder.Entity<Module>()
                .HasMany(r => r.Roles);
            
            base.OnModelCreating(builder);

            //builder.Entity<User>().ToTable("User");
            //builder.Entity<Authorization>().ToTable("Authorization");
            //builder.Entity<Photo>().ToTable("Photo");
            //builder.Entity<Role>().ToTable("Role");
            //builder.Entity<Module>().ToTable("Module");
            
            //builder.Entity<Position>().ToTable("Position");
            //builder.Entity<Department>().ToTable("Department");
            //builder.Entity<Team>().ToTable("Team");

            //builder.Entity<Employee>().ToTable("Employee");
        }
    }
}
