using LoginBackEnd.Domain.Users;

namespace LoginBackEnd.Infrastructure.Users;

public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User(
            Guid.NewGuid(),
            "admin@test.com",
            "123456",  // Esto luego será hash
            0
        )
    };

    public Task<User?> GetByEmailAsync(string email)
    {
        var user = _users.FirstOrDefault(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
        );

        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        return Task.CompletedTask;
    }
}
