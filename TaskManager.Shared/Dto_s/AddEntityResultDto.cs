namespace TaskManager.Shared.Dto_s;

public class AddEntityResultDto
{
    public Guid? Id { get; set; }
    public bool IsAdded { get; set; } = false;
    public string? Token { get; set; }
}
