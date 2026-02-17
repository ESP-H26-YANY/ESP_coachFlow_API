namespace CoachFlowApi.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public int Wallet { get; set; }
    public DateTime CreatedAt { get; set; }


    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Purchase> Purchases { get; set; }

    public User(string email, string password, string name, string role )
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
        Name = name;
        Role = role; 
        Wallet = 0;
        CreatedAt = DateTime.UtcNow; 
        
        Appointments = new List<Appointment>();
        Purchases = new List<Purchase>();
    }

    public User() 
    {
        Appointments = new List<Appointment>();
        Purchases = new List<Purchase>();
    }
}