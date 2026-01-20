using Microsoft.AspNetCore.Mvc;
using IronSec.Models;
using IronSec.Services;
using Google.Cloud.Firestore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IronSec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly FirebaseService _firebase;

        public AuthController()
        {
            _firebase = new FirebaseService();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Hash simples da senha (SHA256)
            user.PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(user.PasswordHash)));

            CollectionReference usersRef = _firebase.Db.Collection("users");
            DocumentReference docRef = usersRef.Document(); // gera ID automaticamente
            user.Id = docRef.Id;

            await docRef.SetAsync(user);

            return Ok(new { message = "User created", user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            login.PasswordHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(login.PasswordHash)));

            CollectionReference usersRef = _firebase.Db.Collection("users");
            Query query = usersRef.WhereEqualTo("Email", login.Email)
                                  .WhereEqualTo("PasswordHash", login.PasswordHash)
                                  .Limit(1);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            if (snapshot.Count == 0)
                return Unauthorized(new { message = "Invalid credentials" });

            var userDoc = snapshot.Documents[0];
            var user = userDoc.ConvertTo<User>();
            return Ok(new { user.Id, user.Name, user.Email, user.Plan });
        }
    }
}