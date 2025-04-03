namespace wrtroad_infraestructureweb.api.core.domain.entities
{
    public class UserRoleEntity
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int RoleId { get; set; } = 3;
        public RoleEntity Role { get; set; }
    }
}
