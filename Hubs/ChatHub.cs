using ChatConnectUltra.Data;
using ChatConnectUltra.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatConnectUltra.Hubs;

public class ChatHub : Hub
{
    private readonly AppDbContext _db;

    public ChatHub(AppDbContext db)
    {
        _db = db;
    }

    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conv-{conversationId}");
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conv-{conversationId}");
    }

    public async Task SendMessage(int conversationId, int senderId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        var message = new Message
        {
            ConversationId = conversationId,
            SenderId = senderId,
            Text = text,
            SentAt = DateTime.Now,
            IsDelivered = true,
            IsRead = false
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var sender = await _db.Users.FirstOrDefaultAsync(u => u.Id == senderId);

        await Clients.Group($"conv-{conversationId}")
            .SendAsync("ReceiveMessage", new
            {
                ConversationId = conversationId,
                SenderId = senderId,
                SenderName = sender?.DisplayName ?? "Unknown",
                Text = message.Text,
                Time = message.SentAt.ToString("t")
            });
    }

    public async Task Typing(int conversationId, int userId)
    {
        await Clients.Group($"conv-{conversationId}")
            .SendAsync("UserTyping", new { ConversationId = conversationId, UserId = userId });
    }
}
