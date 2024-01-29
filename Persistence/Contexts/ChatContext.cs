using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class ChatContext : DbContext
{
    public DbSet<MessageEntity> Messages { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    

    
    public ChatContext()
    {
        
    }


    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseInMemoryDatabase("ChatDb");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ConfigurateUsers(builder);
        ConfigurateMessages(builder);       
        base.OnModelCreating(builder);
    }

    private void ConfigurateMessages(ModelBuilder builder)
    {
        builder.Entity<MessageEntity>().HasKey(x => x.Id);

        builder
            .Entity<MessageEntity>()
            .HasOne(x => x.Sender)
            .WithMany()
            .HasForeignKey(x => x.SenderId);
        
        builder
            .Entity<MessageEntity>()
            .HasOne(x => x.Recipient)
            .WithMany()
            .HasForeignKey(x => x.RecipientId);
    }

    private void ConfigurateUsers(ModelBuilder builder)
    {
        builder.Entity<UserEntity>().HasKey(x => x.Id);
    }

    public async Task SaveAsync(CancellationToken cancellationToken)
    {
        await SaveChangesAsync(cancellationToken);
    }
}
