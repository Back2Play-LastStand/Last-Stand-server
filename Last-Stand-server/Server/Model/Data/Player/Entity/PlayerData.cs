using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Data.Player.Entity;

[Table("last_stand_player_data")]
public class PlayerData
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
    [Column("player_name")]
    [MaxLength(50)]
    public string PlayerName { get; set; } = null!;
}