using TaskManager.Shared.Dto_s.TaskItem;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class TaskItemMapping
{
    public TaskItem AddTaskItemDtoToTaskItem(AddTaskItemDto addTaskItemDto)
    {
        return new TaskItem()
        {
            BoardId = addTaskItemDto.BoardId,
            Title = addTaskItemDto.Title,
            Description = addTaskItemDto.Description,
            AssignedMemberId = addTaskItemDto.AssignedMemberId
        };
    }
    public TaskItemDto TaskItemToTaskItemDto(TaskItem taskItem)
    {
        return new TaskItemDto()
        {
            Id = taskItem.Id,
            BoardId = taskItem.BoardId,
            Title = taskItem.Title,
            Description = taskItem.Description,
            AssignedMemberId = taskItem.AssignedMemberId,
            AssignedMemberName = taskItem.AssignedMemberName,
            Status = taskItem.Status,
            CreatedAt = taskItem.CreatedAt
        };
    }
    public TaskItem UpdateTaskItemDtoTOTaskItem(UpdateTaskItemDto updateTaskItemDto)
    {
        return new TaskItem()
        {
            Title = updateTaskItemDto.Title,
            Description = updateTaskItemDto.Description,
            Status = updateTaskItemDto.Status
        };
    }
}
