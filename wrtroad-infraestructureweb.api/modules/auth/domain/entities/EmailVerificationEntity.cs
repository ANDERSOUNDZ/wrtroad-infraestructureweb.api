using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using wrtroad_infraestructureweb.api.core.domain.entities;

namespace wrtroad_infraestructureweb.api.modules.auth.domain.entities
{
    public class EmailVerificationEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public string VerificationToken { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime ExpirationDate { get; set; }
        public UserEntity User { get; set; }
    }
}
