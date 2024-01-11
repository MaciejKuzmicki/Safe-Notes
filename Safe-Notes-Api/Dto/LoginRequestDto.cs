using System.ComponentModel.DataAnnotations;

namespace Safe_Notes_Api.Dto;

public class LoginRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string TotpCode { get; set; }

}