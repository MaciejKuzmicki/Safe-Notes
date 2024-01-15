using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Dto;

public class NoteCreateDto
{
    [Required]
    public string title { get; set; }
    [Required]
    public string content { get; set; }
    [Required]
    public string password { get; set; }
    [Required]
    public bool encrypted { get; set; }
    [Required]
    public bool ispublic {get; set; }
    
    
    
}