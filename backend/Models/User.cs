using Google.Cloud.Firestore;

namespace IronSec.Models
{
    [FirestoreData]
    public class User
    {
        [FirestoreProperty]
        public string? Id { get; set; }

        [FirestoreProperty]
        public string Email { get; set; } = default!;

        [FirestoreProperty]
        public string Password { get; set; } = default!;
    }
}
