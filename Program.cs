using ChatConnectUltra.Data;
using ChatConnectUltra.Hubs;
using ChatConnectUltra.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=chatconnect.db"));

// MudBlazor
builder.Services.AddMudServices();

// SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Seed data first run
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        var sarah = new User
        {
            DisplayName = "Sarah Johnson",
            EmailOrPhone = "sarah@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            AvatarUrl = "https://i.pravatar.cc/150?img=47",
            StatusMessage = "Working on the dashboard",
            IsOnline = false,
            LastSeen = DateTime.Now
        };

        var tech = new User
        {
            DisplayName = "Tech Team",
            EmailOrPhone = "tech@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            AvatarUrl = "https://i.pravatar.cc/150?img=12",
            StatusMessage = "Deploying new release",
            IsOnline = false,
            LastSeen = DateTime.Now.AddMinutes(-15)
        };

        var david = new User
        {
            DisplayName = "David Martinez",
            EmailOrPhone = "david@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
            AvatarUrl = "https://i.pravatar.cc/150?img=32",
            StatusMessage = "Available",
            IsOnline = false,
            LastSeen = DateTime.Now.AddMinutes(-32)
        };

        db.Users.AddRange(sarah, tech, david);
        db.SaveChanges();

        var conv1 = new Conversation
        {
            User1Id = sarah.Id,
            User2Id = tech.Id,
            CreatedAt = DateTime.Now.AddHours(-5)
        };

        var conv2 = new Conversation
        {
            User1Id = sarah.Id,
            User2Id = david.Id,
            CreatedAt = DateTime.Now.AddHours(-2)
        };

        db.Conversations.AddRange(conv1, conv2);
        db.SaveChanges();

        db.Messages.AddRange(
            new Message
            {
                ConversationId = conv1.Id,
                SenderId = tech.Id,
                Text = "Hey! How's the project coming along?",
                SentAt = DateTime.Now.AddMinutes(-50),
                IsDelivered = true,
                IsRead = true
            },
            new Message
            {
                ConversationId = conv1.Id,
                SenderId = sarah.Id,
                Text = "It's going great! We're ahead of schedule actually.",
                SentAt = DateTime.Now.AddMinutes(-48),
                IsDelivered = true,
                IsRead = true
            },
            new Message
            {
                ConversationId = conv1.Id,
                SenderId = sarah.Id,
                Text = "Here's a preview of the new dashboard design.",
                SentAt = DateTime.Now.AddMinutes(-46),
                IsDelivered = true,
                IsRead = false
            },
            new Message
            {
                ConversationId = conv2.Id,
                SenderId = david.Id,
                Text = "Hi! I've finished the initial mockups.",
                SentAt = DateTime.Now.AddMinutes(-40),
                IsDelivered = true,
                IsRead = false
            },
            new Message
            {
                ConversationId = conv2.Id,
                SenderId = sarah.Id,
                Text = "Great! Can you share them?",
                SentAt = DateTime.Now.AddMinutes(-38),
                IsDelivered = true,
                IsRead = false
            }
        );
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapHub<ChatHub>("/chatHub");
app.MapFallbackToPage("/_Host");

app.Run();
