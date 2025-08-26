namespace TaskManager.Shared.Dto_s.Authentication;

public class AuthenticationResultDto
{
    public bool IsSuccessful { get; set; } = false;
    public string? ErrorMessage { get; set; }
    public string? Token { get; set; }
}
