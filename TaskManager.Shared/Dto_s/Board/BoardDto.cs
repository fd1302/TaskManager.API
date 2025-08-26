namespace TaskManager.Shared.Dto_s.Board;

public class BoardDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
