using MessangerBackend.Core.Interfaces;

namespace MessangerBackend.Core.Models;

public class User: IUserWithNickname
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string Password { get; set; }
    
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime LastSeenOnline { get; set; }
    public virtual ICollection<Chat> Chats { get; set; }
}