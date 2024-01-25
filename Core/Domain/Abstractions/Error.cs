namespace Domain.Abstractions;

public abstract class Error
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
