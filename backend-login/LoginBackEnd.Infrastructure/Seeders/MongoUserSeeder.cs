using LoginBackEnd.Domain.Users;
using MongoDB.Driver;

namespace LoginBackEnd.Infrastructure.Seeders
{
    public class MongoUserSeeder
    {
        private readonly IMongoCollection<User> _collection;

        public MongoUserSeeder(IMongoDatabase database)
        {
            _collection = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Ejecuta la siembra de datos iniciales. 
        /// </summary>
        public async Task SeedAsync()
        {
            // Verificar si ya existe un admin
            var exists = await _collection.Find(u => u.Email == "admin@test.com").AnyAsync();
            if (!exists)
            {
                var admin = new User(
                    id: Guid.NewGuid(),
                    email: "admin@test.com",
                    passwordHash: "123456", // Esto debe ser un hash en producción
                    loginAttempts: 0,
                    isBlocked: false,
                    blockedUntil: null
                );
                await _collection.InsertOneAsync(admin);
            }

            // Verificar si ya existe
            exists = await _collection.Find(u => u.Email == "leonardo@test.com").AnyAsync();
            if (!exists)
            {
                var user1 = new User(
                    id: Guid.NewGuid(),
                    email: "leonardo@test.com",
                    passwordHash: "leo123",
                    loginAttempts: 0,
                    isBlocked: false,
                    blockedUntil: null
                );
                await _collection.InsertOneAsync(user1);
            }

            // Verificar si ya existe
            exists = await _collection.Find(u => u.Email == "duvan@test.com").AnyAsync();
            if (!exists)
            {
                var user1 = new User(
                    id: Guid.NewGuid(),
                    email: "duvan@test.com",
                    passwordHash: "duvan123",
                    loginAttempts: 0,
                    isBlocked: false,
                    blockedUntil: null
                );
                await _collection.InsertOneAsync(user1);
            }

            // Verificar si ya existe
            exists = await _collection.Find(u => u.Email == "evelio@test.com").AnyAsync();
            if (!exists)
            {
                var user1 = new User(
                    id: Guid.NewGuid(),
                    email: "evelio@test.com",
                    passwordHash: "evelio123",
                    loginAttempts: 0,
                    isBlocked: false,
                    blockedUntil: null
                );
                await _collection.InsertOneAsync(user1);
            }
            // Verificar si ya existe
            exists = await _collection.Find(u => u.Email == "emoralesan@cenfotec.ac.cr").AnyAsync();
            if (!exists)
            {
                var user1 = new User(
                    id: Guid.NewGuid(),
                    email: "emoralesan@ucenfotec.ac.cr",
                    passwordHash: "evelio123",
                    loginAttempts: 0,
                    isBlocked: false,
                    blockedUntil: null
                );
                await _collection.InsertOneAsync(user1);
            }
        }
    }
}
