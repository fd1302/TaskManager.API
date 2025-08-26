namespace TaskManager.Shared.Dto_s;

public class EmailValidationDto
{
    public string email { get; set; } = string.Empty;
    public string deliverability { get; set; } = string.Empty;
    public IsValidFormat? is_valid_format { get; set; }

    public class IsValidFormat
    {
        public bool value { get; set; }
        public string text { get; set; } = string.Empty;
    }

}
