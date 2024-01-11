using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Models;

public class Note
{
    public Guid NoteId { get; set; }
    
    public Guid UserId { get; set; }
    [Required]
    public string content { get; set; }

    public bool isEncrypted { get; set; } = false;
    
    public virtual User User { get; set; }
    
}