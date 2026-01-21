using Google.Cloud.Firestore;
using IronSec.Models;
using BCrypt.Net;

namespace IronSec.Services
{
    public class AuthService
    {
        private readonly FirestoreDb _db;

        public AuthService()
        {
            var firebase = new FirebaseService();
            _db = firebase.Db;
        }

        public async Task<User> Register(User user)
        {
            // Gera ID se não existir
            user.Id ??= Guid.NewGuid().ToString();

            // 🔐 Gera hash da senha
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var docRef = _db.Collection("users").Document(user.Id);
            await docRef.SetAsync(user);

            // Nunca retorne a senha
            user.Password = "";

            return user;
        }

        public async Task<User?> Login(string email, string password)
        {
            var query = _db.Collection("users")
                .WhereEqualTo("Email", email);

            var snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
                return null;

            var user = snapshot.Documents[0].ConvertTo<User>();

            // 🔐 Verifica hash
            bool passwordOk = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!passwordOk)
                return null;

            // Nunca retorne a senha
            user.Password = "";

            return user;
        }
    }
}
