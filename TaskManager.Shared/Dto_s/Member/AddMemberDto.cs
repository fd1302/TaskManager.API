namespace TaskManager.Shared.Dto_s.Member;

public class AddMemberDto
{
    public Guid? TenantId { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
