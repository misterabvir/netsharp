namespace Domain;


public class UserEntity : Entity
{      
    public required string Name { get; set; }
    public DateTime LastOnline { get; set; }
}
