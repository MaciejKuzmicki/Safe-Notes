using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Dto;

public class NoteCreateDto
{
    [Required]
    public string content { get; set; }
    [Required]
    public string password { get; set; }
    
    
}