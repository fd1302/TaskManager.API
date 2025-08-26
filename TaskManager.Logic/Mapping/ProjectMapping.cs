using TaskManager.Shared.Dto_s.Project;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class ProjectMapping
{
    public Project AddProjectDtoToProject(AddProjectDto addProjectDto)
    {
        return new Project()
        {
            Name = addProjectDto.Name,
            Description = addProjectDto.Description
        };
    }
    public ProjectDto ProjectToProjectDto(Project project)
    {
        return new ProjectDto()
        {
            Id = project.Id,
            TenantId = project.TenantId,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt
        };
    }
    public Project UpdateProjectDtoTOProject(UpdateProjectDto updateProjectDto)
    {
        return new Project()
        {
            Name = updateProjectDto.Name,
            Description = updateProjectDto.Description
        };
    }
}
