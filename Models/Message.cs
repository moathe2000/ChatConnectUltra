namespace ChatConnectUltra.Models;

public class Message
{
    public int Id { get; set; }
    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string Text { get; set; } = "";
    public DateTime SentAt { get; set; } = DateTime.Now;
    public bool IsDelivered { get; set; }
    public bool IsRead { get; set; }

    public Conversation? Conversation { get; set; }
    public User? Sender { get; set; }
}
