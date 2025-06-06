namespace KursovayaServer.Models;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime Created { get; set; }
    public string UserId { get; set; } = "";
} 