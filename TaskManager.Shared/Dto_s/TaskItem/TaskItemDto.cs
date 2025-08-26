namespace TaskManager.Shared.Dto_s.TaskItem;

public class TaskItemDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AssignedMemberId { get; set; }
    public string Status { get; set; } = "InProgress";
    public string CreatedAt { get; set; } = string.Empty;
}
