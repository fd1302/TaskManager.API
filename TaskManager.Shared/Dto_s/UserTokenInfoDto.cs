namespace TaskManager.Shared.Dto_s;

public class UserTokenInfoDto
{
    public Guid Id { get; set; }
    public Guid? TenantId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
