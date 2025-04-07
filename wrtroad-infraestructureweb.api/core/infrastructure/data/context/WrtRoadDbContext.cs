using Microsoft.EntityFrameworkCore;
using wrtroad_infraestructureweb.api.core.domain.entities;
using wrtroad_infraestructureweb.api.core.infrastructure.persistence.seeders;
using wrtroad_infraestructureweb.api.modules.auth.domain.entities;

namespace wrtroad_infraestructureweb.api.core.infrastructure.data.context
{
    public class WrtRoadDbContext : DbContext
    {
        public WrtRoadDbContext(DbContextOptions<WrtRoadDbContext> options) : base(options)
        { }
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<RoleEntity> Roles { get; set; }
        public virtual DbSet<UserRoleEntity> UserRoles { get; set; }
        public virtual DbSet<EmailVerificationEntity> EmailVerifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedConfiguration();
            modelBuilder.Entity<UserRoleEntity>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRoleEntity>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
            modelBuilder.Entity<EmailVerificationEntity>()
                .HasOne(ev => ev.User)
                .WithOne()
                .HasForeignKey<EmailVerificationEntity>(ev => ev.UserId);
        }
    }
}
