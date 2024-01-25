using Domain.Abstractions;

namespace Domain.Models;

public sealed class User : Entity
{
    public required string Name { get; set; }
}
