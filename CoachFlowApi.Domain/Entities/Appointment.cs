namespace CoachFlowApi.Domain.Entities;

public class Appointment
{
    public Guid Id { get; set; }
    public Guid CoachId { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Duration { get; set; }

    public Coach Coach { get; set; }
    public User User { get; set; }

    public Appointment(Guid coachId, Guid userId, DateTime date, string title, string description, int duration)
    {
        Id = Guid.NewGuid();
        CoachId = coachId;
        UserId = userId;
        Date = date;
        Title = title;
        Description = description;
        Duration = duration;
    }

    public Appointment() { }
}