using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginBackEnd.Domain.Users;

public class User
{
    /// Identificador único del usuario (Mongo usará este campo como _id).
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; }
    public string Email { get;  }
    public string PasswordHash { get;  }

    public int LoginAttempts { get; private set; }
    public bool IsBlocked { get; private set; }
    public DateTime? BlockedUntil { get; private set; }

    public User(Guid id, string email, string passwordHash, int loginAttempts = 0, bool isBlocked = false, DateTime? blockedUntil = null)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
        LoginAttempts = loginAttempts;
        IsBlocked = isBlocked;
        BlockedUntil = blockedUntil;
    }

    public void IncrementLoginAttempts()
    {
        LoginAttempts++;
    }

    public void ResetLoginAttempts()
    {
        LoginAttempts = 0;
    }

    public void Block(int minutes)
    {
        IsBlocked = true;
        BlockedUntil = DateTime.UtcNow.AddMinutes(minutes);
    }

    public void Unblock()
    {
        IsBlocked = false;
        BlockedUntil = null;
        LoginAttempts = 0;
    }

    public bool IsStillBlocked()
    {
        if (!IsBlocked) return false;
        if (BlockedUntil.HasValue && BlockedUntil.Value > DateTime.UtcNow) return true;
        
        // Si ya expiró el bloqueo
        Unblock();
        return false;
    }
}
