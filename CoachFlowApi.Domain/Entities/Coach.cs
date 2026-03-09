namespace CoachFlowApi.Domain.Entities;

public class Coach
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } 
    public string Specialization { get; set; }

    public virtual User User { get; set; }
    public virtual ICollection<Guide> Guides { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; }

    public Coach(Guid userId, string specialization)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Specialization = specialization;
        
        Guides = new List<Guide>();
        Appointments = new List<Appointment>();
    }

    public Coach() 
    {
        Guides = new List<Guide>();
        Appointments = new List<Appointment>();
    }
}