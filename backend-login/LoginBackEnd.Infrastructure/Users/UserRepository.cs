using LoginBackEnd.Domain.Users;
using MongoDB.Driver;

namespace LoginBackEnd.Infrastructure.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase database)
        {
            // Nombre de la colección: "Users"
            _collection = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// Obtiene un usuario por su email.
        /// </summary>
        /// <param name="email">Correo del usuario.</param>
        /// <returns>El usuario o null si no existe.</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            return await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Inserta un usuario nuevo en MongoDB.
        /// </summary>
        /// <param name="user">Entidad de usuario.</param>
        public async Task AddAsync(User user)
        {
            await _collection.InsertOneAsync(user);
        }

        /// <summary>
        /// Verifica si existe un usuario por email. 
        /// </summary>
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);

            return await _collection.Find(filter).AnyAsync();
        }

        /// <summary>
        /// Lista todos los usuarios (para pruebas).
        /// </summary>
        public async Task<List<User>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        /// <summary>
        /// Actualiza un usuario completo en MongoDB. Se actualiza el conteo de intentos de login.
        /// Se usa ReplaceOneAsync porque guardas la entidad Domain directamente.
        /// </summary>
        public async Task UpdateAsync(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, user.Email);

            await _collection.ReplaceOneAsync(filter, user);
        }
    }
}
