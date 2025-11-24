namespace ChatConnectUltra.Models;

public class User
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = "";
    public string EmailOrPhone { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string? AvatarUrl { get; set; }
    public string StatusMessage { get; set; } = "";
    public bool IsOnline { get; set; }
    public DateTime? LastSeen { get; set; }

    public ICollection<Conversation> ConversationsAsUser1 { get; set; } = new List<Conversation>();
    public ICollection<Conversation> ConversationsAsUser2 { get; set; } = new List<Conversation>();
}
