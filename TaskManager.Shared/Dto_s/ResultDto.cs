namespace TaskManager.Shared.Dto_s;

public class ResultDto
{
    public bool Success { get; set; } = false;
    public string Operation { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
