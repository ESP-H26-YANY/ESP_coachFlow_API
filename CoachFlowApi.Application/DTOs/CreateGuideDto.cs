namespace CoachFlowApi.Application.DTOS;

public class CreateGuideDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int Price { get; set; }
    public string LinkUrl { get; set; } 
    public string CoverUrl { get; set; }

    public CreateGuideDto() { }
}