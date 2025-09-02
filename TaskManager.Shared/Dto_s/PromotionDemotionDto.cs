namespace TaskManager.Shared.Dto_s;

public class PromotionDemotionDto
{
    public Guid UserId { get; set; }
    public string Role { get; set; } = string.Empty;
}
