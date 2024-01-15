using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Models;

public class Note
{
    [Key]
    public Guid NoteId { get; set; }
    
    public Guid UserId { get; set; }
    [Required]
    public string title { get; set; }
    [Required]
    public string content { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }

    public bool isEncrypted { get; set; } = false;

    public bool isPublic { get; set; } = false;
    
    public virtual User User { get; set; }
    
}