namespace CoachFlowApi.Domain.Entities;

public class Coach
{
    public Guid Id { get; set; }
    public string Specialization { get; set; }

    public ICollection<Guide> Guides { get; set; }
    public ICollection<Appointment> Appointments { get; set; }


    public Coach(string specialization)
    {
        Id = Guid.NewGuid();
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