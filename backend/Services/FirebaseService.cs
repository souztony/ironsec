using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace IronSec.Services
{
    public class FirebaseService
    {
        public FirestoreDb Db { get; private set; }

        public FirebaseService()
        {
            var credentialPath = Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "ironsec-firebase.json"
            );

            var credential = GoogleCredential.FromFile(credentialPath);

            var builder = new FirestoreDbBuilder
            {
                ProjectId = "iron-sec",
                Credential = credential
            };

            Db = builder.Build();
        }
    }
}
