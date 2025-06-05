using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagmentService.Models;

namespace Infrastructure.Appdbcontext
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole,   string>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RolePrivilege>()
                .HasKey(rp => new { rp.RoleId, rp.PrivilegeId });

            modelBuilder.Entity<RolePrivilege>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePrivileges)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<RolePrivilege>()
                .HasOne(rp => rp.Privilege)
                .WithMany(p => p.RolePrivileges)
                .HasForeignKey(rp => rp.PrivilegeId);


            modelBuilder.Entity<UserRole>(role =>
            {
                role.HasKey(r => r.Id);
                role.Property(r => r.RoleName).IsRequired().HasMaxLength(256);
                role.Property(r => r.Description).HasMaxLength(500);


            });

            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityRole>();

            modelBuilder.Entity<UserRoleMapping>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                // Optionally, configure additional properties or relationships here
                userRole.Property(ur => ur.AssignDate).IsRequired();
                userRole.Property(ur => ur.AssignedBy).HasMaxLength(200);
            });



        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<RolePrivilege> RolePrivileges { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

    }
}
