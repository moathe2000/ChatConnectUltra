using ChatConnectUltra.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatConnectUltra.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
