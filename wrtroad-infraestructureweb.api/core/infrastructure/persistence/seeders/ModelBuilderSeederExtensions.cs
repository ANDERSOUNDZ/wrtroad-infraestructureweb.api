using Microsoft.EntityFrameworkCore;
using wrtroad_infraestructureweb.api.core.domain.entities;

namespace wrtroad_infraestructureweb.api.core.infrastructure.persistence.seeders
{
    public static class ModelBuilderSeederExtensions
    {
        public static void SeedConfiguration(this ModelBuilder modelBuilder)
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
            modelBuilder.Entity<UserRoleEntity>().HasData(
               new UserRoleEntity()
               {
                   UserId = 1,
                   RoleId = 1
               }
           );           
        }
    }
}
