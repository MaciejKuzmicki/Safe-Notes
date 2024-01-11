using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Models;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public byte[] PasswordHash { get; set; }
    [Required]
    public byte[] PasswordSalt { get; set; }
    [Required]
    public byte[] TOTPSecret { get; set; }
    
    public ICollection<Note> Notes { get; set; }
    
}