using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace wrtroad_infraestructureweb.api.core.domain.entities
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(60)]
        public string? Name { get; set; }
        [StringLength(60)]
        public string? Surname { get; set; }
        [Required]
        [StringLength(60)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 6)]
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime DateRegister { get; set; }
        public ICollection<UserRoleEntity> UserRoles { get; set; } = new List<UserRoleEntity>();
    }
}
