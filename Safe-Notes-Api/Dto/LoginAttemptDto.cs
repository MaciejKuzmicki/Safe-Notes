namespace Safe_Notes_Api.Dto;

public class LoginAttemptDto
{
    public bool Success { get; set; }
    public string IpAddress { get; set; }
    public DateTime Time { get; set; }
    
    public string ClientName { get; set; }
    
}