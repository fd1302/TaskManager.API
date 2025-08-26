namespace TaskManager.Shared.Dto_s.TaskItem;

public class AddTaskItemDto
{
    public Guid BoardId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AssignedMemberId { get; set; }
}
