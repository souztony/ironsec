using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace IronSec.Services
{
    public class FirebaseService
    {
        public FirestoreDb Db { get; private set; }

        public FirebaseService()
        {
            
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("Config/ironsec-firebase.json")
                });
            }

            Db = FirestoreDb.Create("iron-sec");
        }
    }
}