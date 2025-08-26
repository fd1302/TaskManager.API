using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Project;

namespace TaskManager.Logic.AppManager;

public class ProjectManager
{
    private readonly ProjectRepository _projectRepository;
    private readonly ProjectMapping _projectMapping;
    public ProjectManager(ProjectRepository projectRepository, ProjectMapping projectMapping)
    {
        _projectRepository = projectRepository ??
            throw new ArgumentNullException(nameof(projectRepository));
        _projectMapping = projectMapping ??
            throw new ArgumentNullException(nameof(projectMapping));
    }
    public async Task<AddEntityResultDto> AddAsync(Guid tenantId, AddProjectDto addProjectDto)
    {
        if (string.IsNullOrEmpty(addProjectDto.Name))
        {
            throw new ArgumentNullException("Name can't be empty.");
        }
        else if (addProjectDto.Name.Length == 0 || addProjectDto.Name.Length > 100)
        {
            throw new ArgumentException("Name should atleast have 1 character and can't be more than 100 characters.");
        }
        else if (addProjectDto.Description.Length > 300)
        {
            throw new ArgumentException("Description can't be more than 300 characters.");
        }
        var project = _projectMapping.AddProjectDtoToProject(addProjectDto);
        project.Id = Guid.NewGuid();
        project.TenantId = tenantId;
        project.CreatedAt = DateTime.Now.ToString().Substring(0, 9);
        var result = await _projectRepository.AddAsync(project);
        return new AddEntityResultDto()
        {
            IsAdded = true
        };
    }
    public async Task<IEnumerable<ProjectDto>?> GetProjectsAsync(string? searchQuery)
    {
        var projects = await _projectRepository.GetProjectsAsync(searchQuery);
        if (projects == null)
        {
            return null;
        }
        var mappedProjects = projects.Select(_projectMapping.ProjectToProjectDto);
        return mappedProjects;
    }
    public async Task<ProjectDto?> GetProjectAsync(Guid id)
    {
        var project = await _projectRepository.GetProjectAsync(id);
        if (project == null)
        {
            return null;
        }
        var mappedProject = _projectMapping.ProjectToProjectDto(project);
        return mappedProject;
    }
    public async Task<IEnumerable<ProjectDto>?> GetProjectsWithTenantIdAsync(Guid id)
    {
        var projects = await _projectRepository.GetProjectsWithTenantIdAsync(id);
        if (projects == null)
        {
            return null;
        }
        var mappedProjects = projects.Select(_projectMapping.ProjectToProjectDto);
        return mappedProjects;
    }
    public async Task<ResultDto> UpdateAsync(Guid id, UpdateProjectDto updateProjectDto)
    {
        bool projectExists = await _projectRepository.ProjectExistsAsync(id);
        if (!projectExists)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "Project not found."
            };
        }
        else if (updateProjectDto.Name != null)
        {
            if (updateProjectDto.Name.Length > 100)
            {
                throw new ArgumentException("Project name can't be more than 100 chracters.");
            }
        }
        if (updateProjectDto.Description != null)
        {
            if (updateProjectDto.Description.Length > 300)
            {
                throw new ArgumentException("Description can't be more than 300 characters.");
            }
        }
        var project = _projectMapping.UpdateProjectDtoTOProject(updateProjectDto);
        var result = await _projectRepository.UpdateAsync(id, project);
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
            Success = true,
            Operation = "Update",
            Message = "Updated successfuly."
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        bool projectExists = await _projectRepository.DeleteAsync(id);
        if (!projectExists)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Project not found."
            };
        }
        var result = await _projectRepository.DeleteAsync(id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Delete",
            Message = "Deleted successfuly."
        };
    }
}
