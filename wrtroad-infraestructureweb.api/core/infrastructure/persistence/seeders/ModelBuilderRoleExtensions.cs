using Microsoft.EntityFrameworkCore;
using wrtroad_infraestructureweb.api.core.domain.entities;

namespace wrtroad_infraestructureweb.api.core.infrastructure.persistence.seeders
{
    public static class ModelBuilderRoleExtensions
    {
        public static void SeedRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity()
                {
                    Id = 1,
                    Name = "super-admin"
                },
                new RoleEntity()
                {
                    Id = 2,
                    Name = "mid-admin"
                },
                new RoleEntity()
                {
                    Id = 3,
                    Name = "client-writter"
                }
            );
        }
    }
}
