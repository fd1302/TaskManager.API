namespace TaskManager.Shared.Dto_s.Manager;

public class AddManagerDto
{
    public required Guid TenantId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
