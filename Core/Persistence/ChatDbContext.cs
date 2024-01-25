using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Persistence;

public class ChatDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=DB\\Chat.db");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        ConfigurateUsers(builder);
        ConfigurateMessages(builder);

        base.OnModelCreating(builder);
    }

    private void ConfigurateMessages(ModelBuilder builder)
    {
        builder.Entity<Message>().ToTable("Messages");
        builder.Entity<Message>().HasKey(u => u.Id);
        builder.Entity<Message>().Property(u => u.Content).IsRequired().HasColumnName("Content");
        builder.Entity<Message>().Property(u => u.SenderId).IsRequired().HasColumnName("Sender");
        builder.Entity<Message>().Property(u => u.RecipientId).HasColumnName("Recepient");
        builder.Entity<Message>().Property(u => u.DateTime).HasColumnName("Date");

        builder.Entity<Message>().Ignore(u => u.Command);
        builder.Entity<Message>().Ignore(u => u.Error);
        builder.Entity<Message>().Ignore(u => u.MessageType);
    }

    private void ConfigurateUsers(ModelBuilder builder)
    {
        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Name).IsRequired().HasColumnName("Username");
    }
}
