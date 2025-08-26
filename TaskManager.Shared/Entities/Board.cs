namespace TaskManager.Shared.Entities;

public class Board
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
