using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.TaskItem;

namespace TaskManager.Logic.AppManager;

public class TaskItemManager
{
    private readonly TaskItemRepository _taskItemRepository;
    private readonly TaskItemMapping _taskItemMapping;
    private readonly BoardRepository _boardRepository;
    private readonly MemberRepository _memberRepository;
    public TaskItemManager(TaskItemRepository taskItemRepository, TaskItemMapping taskItemMapping, BoardRepository boardRepository,
        MemberRepository memberRepository)
    {
        _taskItemRepository = taskItemRepository ??
            throw new ArgumentNullException(nameof(taskItemRepository));
        _taskItemMapping = taskItemMapping ??
            throw new ArgumentNullException(nameof(taskItemMapping));
        _boardRepository = boardRepository ??
            throw new ArgumentNullException(nameof(boardRepository));
        _memberRepository = memberRepository ??
            throw new ArgumentNullException(nameof(memberRepository));
    }
    public async Task<AddEntityResultDto> AddAsync(AddTaskItemDto addTaskItemDto)
    {
        bool boardExists = await _boardRepository.BoardExistsAsync(addTaskItemDto.BoardId);
        bool memberExists = await _memberRepository.MemberExistsAsync(null, addTaskItemDto.AssignedMemberId);
        if (!boardExists)
        {
            throw new ArgumentNullException("Board not found.");
        }
        else if (!memberExists)
        {
            throw new ArgumentNullException("Member not found.");
        }
        else if (string.IsNullOrEmpty(addTaskItemDto.Title))
        {
            throw new ArgumentNullException("Task title can't be empty.");
        }
        else if (addTaskItemDto.Title.Length > 100)
        {
            throw new ArgumentException("Title can't be more than 100 characters.");
        }
        else if (string.IsNullOrEmpty(addTaskItemDto.Description))
        {
            throw new ArgumentNullException("Description can't be empty.");
        }
        else if (addTaskItemDto.Description.Length > 300)
        {
            throw new ArgumentException("Description can't be more than 300 characters.");
        }
        var taskItem = _taskItemMapping.AddTaskItemDtoToTaskItem(addTaskItemDto);
        taskItem.Id = Guid.NewGuid();
        taskItem.CreatedAt = DateTime.UtcNow.ToString().Substring(0, 9);
        var result = await _taskItemRepository.AddAsync(taskItem);
        if (!result)
        {
            throw new ArgumentException("There was an error while handling the request.");
        }
        return new AddEntityResultDto()
        {
            Id = taskItem.Id,
            IsAdded = true
        };
    }
    public async Task<IEnumerable<TaskItemDto>?> GetTaskItemsAsync(string? searchQuery)
    {
        var taskItems = await _taskItemRepository.GetTaskItemsAsync(searchQuery);
        if (taskItems == null)
        {
            return null;
        }
        var mappedTaskItems = taskItems.Select(_taskItemMapping.TaskToTaskItemDto);
        return mappedTaskItems;
    }
    public async Task<TaskItemDto?> GetTaskItemAsync(Guid id)
    {
        var taskItem = await _taskItemRepository.GetTaskItemAsync(id);
        if (taskItem == null)
        {
            return null;
        }
        var mappedTaskItem = _taskItemMapping.TaskToTaskItemDto(taskItem);
        return mappedTaskItem;
    }
    public async Task<IEnumerable<TaskItemDto>?> GetTaskItemsWithBoardIdAsync(Guid id)
    {
        var taskItems = await _taskItemRepository.GetTaskItemsWithBoardIdAsync(id);
        if (taskItems == null)
        {
            return null;
        }
        var mappedTaskItems = taskItems.Select(_taskItemMapping.TaskToTaskItemDto);
        return mappedTaskItems;
    }
    public async Task<ResultDto> UpdateAsync(Guid id, UpdateTaskItemDto updateTaskItemDto)
    {
        var taskItemExists = await _taskItemRepository.TaskItemExistsAsync(id);
        if (!taskItemExists)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "Task item not found."
            };
        }
        if (updateTaskItemDto.Title != null)
        {
            if (updateTaskItemDto.Title.Length > 100)
            {
                throw new ArgumentException("Title can't be more than 100 characters.");
            }
        }
        if (updateTaskItemDto.Description != null)
        {
            if (updateTaskItemDto.Description.Length > 300)
            {
                throw new ArgumentException("Description can't be more than 300 characters.");
            }
        }
        if (updateTaskItemDto.Status != null)
        {
            if (updateTaskItemDto.Status.Length > 30)
            {
                throw new ArgumentException("Status can't be more than 30 characters.");
            }
        }
        var taskItem = _taskItemMapping.UpdateTaskItemDtoTOTaskItem(updateTaskItemDto);
        bool result = await _taskItemRepository.UpdateAsync(id, taskItem);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "There was an error while handling the request."
            };
        }
        return new ResultDto()
        {
            Operation = "Update",
            Message = "Updated successfuly.",
            Success = true
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var taskItemExists = await _taskItemRepository.TaskItemExistsAsync(id);
        if (!taskItemExists)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Task item not found."
            };
        }
        var result = await _taskItemRepository.DeleteAsync(id);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "There was an error while handling the request."
            };
        }
        return new ResultDto()
        {
            Operation = "Delete",
            Message = "Deleted successfuly.",
            Success = true
        };
    }
}
