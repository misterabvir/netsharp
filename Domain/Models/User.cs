using Domain.Abstractions;

namespace Domain.Models;

public sealed class User : Entity
{
    public string Name { get; set; }
}
