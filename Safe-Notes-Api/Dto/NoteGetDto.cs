namespace Safe_Notes_Api.Dto;

public class NoteGetDto
{
    public string content { get; set; }
    public string title { get; set; }
    public bool encrypted { get; set; }
    
}