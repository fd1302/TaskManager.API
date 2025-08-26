namespace TaskManager.Shared.Dto_s.Admin;

public class AddAdminDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
}
