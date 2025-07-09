using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Model.Account.Entity;

[Table("account_session")]
public class AccountSession
{
    public string SessionId { get; set; } = null!;
    public int AccountId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
}