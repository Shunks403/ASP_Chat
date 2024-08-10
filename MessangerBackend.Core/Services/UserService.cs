using System.Security.Cryptography;
using System.Text;
using MessangerBackend.Core.Interfaces;
using MessangerBackend.Core.Models;

namespace MessangerBackend.Core.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;

    public UserService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<User> Login(string nickname, string password)
    {
        var user = await _repository.GetByNicknameAsync<User>(nickname);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid nickname or password.");
        }

        if (!VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedAccessException("Invalid nickname or password.");
        }

        user.LastSeenOnline = DateTime.UtcNow;
        _repository.Update(user);

        return user;
    }

    public async Task<User> Register(string nickname, string password)
    {
        
        if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Nickname and password cannot be empty.");
        }

        if (password.Length < 6)
        {
            throw new ArgumentException("Password must be at least 6 characters long.");
        }

        if (await _repository.GetByNicknameAsync<User>(nickname) != null)
        {
            throw new ArgumentException("Nickname is already taken.");
        }

        var (passwordHash, passwordSalt) = CreatePasswordHash(password);

        var user = new User
        {
            Nickname = nickname,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTime.UtcNow,
            LastSeenOnline = DateTime.UtcNow,
            Chats = new List<Chat>() 
        };

        
        await _repository.Add(user);

        return user;
    }

    public Task<User> GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> GetUsers(int page, int size)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<User> SearchUsers(string nickname)
    {
        throw new NotImplementedException();
    }
    
   
    private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
    {
        using (var hmac = new HMACSHA512())
        {
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return (hash, salt);
        }
    }

   
    private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}