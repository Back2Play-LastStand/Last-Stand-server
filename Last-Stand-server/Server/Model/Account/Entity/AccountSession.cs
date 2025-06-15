using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Account.Entity;

[Table("account_session")]
public class AccountSession
{
    [Key]
    [Column("session_id")]
    [MaxLength(64)]
    public string SessionId { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [Column("account_id")]
    public int AccountId { get; set; }
    
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }
}