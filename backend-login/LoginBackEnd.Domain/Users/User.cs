using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LoginBackEnd.Domain.Users;
public class User
{
    /// Identificador único del usuario (Mongo usará este campo como _id).
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; }
    public string Email { get; }
    public string PasswordHash { get; }

    public User(Guid id, string email, string passwordHash)
    {
        Id = id;
        Email = email;
        PasswordHash = passwordHash;
    }
}
