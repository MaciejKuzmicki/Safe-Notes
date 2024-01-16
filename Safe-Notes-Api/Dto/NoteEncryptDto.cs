using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Dto;

public class NoteEncryptDto
{
    [Required]
    public string password { get; set; }
    
}