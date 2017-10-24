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
        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Employee> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<RoleModule>().HasKey(rm => new { rm.RoleId, rm.ModuleId });
            builder.Entity<RoleModule>().HasOne(r => r.Role).WithMany(b => b.RoleModules);
            builder.Entity<RoleModule>().HasOne(m => m.Module).WithMany(m => m.RoleModules);
            builder.Entity<User>().HasOne(m => m.Role).WithMany(r => r.Users);
            builder.Entity<User>().HasOne(m => m.Employee).WithMany(r => r.Users);
            builder.Entity<Position>().HasOne(d => d.Department).WithMany(d => d.Positions);
            builder.Entity<Team>().HasOne(t => t.Department).WithMany(d => d.Teams);
            builder.Entity<Team>().HasOne(t => t.Employee).WithMany(e => e.Teams);
            base.OnModelCreating(builder);

        }
    }
}
