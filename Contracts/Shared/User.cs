using Domain;
using System.Net;
using System.Text.Json.Serialization;

namespace Contracts.Shared;

public record User
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public IPEndPoint? EndPoint { get; set; }
    public DateTime LastOnline { get; set; } = DateTime.Now;

    public UserEntity ToDomain() => new(){ Id = Id, Name = Name, LastOnline = LastOnline };

    public static User? FromDomain(UserEntity? userEntity)
        => userEntity is null ? null 
        : new User { Id = userEntity.Id,  Name = userEntity.Name, LastOnline = userEntity.LastOnline };
}
