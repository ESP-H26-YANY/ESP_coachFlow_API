namespace CoachFlowApi.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
    public int Wallet { get; set; }
    public DateTime? LastClaimDate { get; set; }
    public DateTime CreatedAt { get; set; }


    public ICollection<Purchase> Purchases { get; set; }
    public virtual ICollection<SavedGuide> SavedGuides { get; set; } = new List<SavedGuide>();

    public User(string email, string password, string name, string role )
    {
        Id = Guid.NewGuid();
        Email = email;
        Password = password;
        Name = name;
        Role = role; 
        Wallet = 0;
        CreatedAt = DateTime.UtcNow; 
        
        Purchases = new List<Purchase>();
    }

    public User() 
    {
        Purchases = new List<Purchase>();
    }
}