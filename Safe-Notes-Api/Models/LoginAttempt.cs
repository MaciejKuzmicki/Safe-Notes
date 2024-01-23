using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safe_Notes_Api.Models;

public class LoginAttempt
{
    [Key]
    public Guid LoginAttemptId { get; set; }
    public bool Success { get; set; }
    public string IpAddress { get; set; }
    
    public string ClientName { get; set; }
    public DateTime Time { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
}