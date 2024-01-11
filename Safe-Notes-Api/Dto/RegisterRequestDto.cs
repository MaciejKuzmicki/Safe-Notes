using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Dto;

public class RegisterRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    
}