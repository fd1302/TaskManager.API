namespace TaskManager.Shared.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AssignedMemberId { get; set; }
    public string AssignedMemberName { get; set; } = string.Empty;
    public string Status { get; set; } = "InProgress";
    public string CreatedAt { get; set; } = string.Empty;
}
