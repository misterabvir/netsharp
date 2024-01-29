using System.ComponentModel.DataAnnotations;

namespace Domain;

public abstract class Entity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
