using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Safe_Notes_Api.Models;

public class Note
{
    [Key]
    public Guid NoteId { get; set; }
    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    [Required]
    public string title { get; set; }
    [Required]
    public string content { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string iv { get; set; }
    public bool isEncrypted { get; set; } = false;

    public bool isPublic { get; set; } = false;
    
    public virtual User User { get; set; }
    
}