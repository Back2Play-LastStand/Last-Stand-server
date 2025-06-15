using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Account.Entity;

[Table("player_account_data")]
public class PlayerLoginData
{
    [Key]
    [Required]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("player_id")]
    [MaxLength(15)]
    public string PlayerId { get; set; } = null!;
    
    [Required]
    [Column("password")]
    [MaxLength(60)]
    public string Password { get; set; } = null!;
    
    [Required]
    [Column("email")]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required] [Column("is_new_account")]
    public bool IsNewAccount { get; set; }
}