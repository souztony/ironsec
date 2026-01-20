using Microsoft.AspNetCore.Mvc;
using IronSec.Models;
using IronSec.Services;
using Google.Cloud.Firestore;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace IronSec.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SiteController : ControllerBase
    {
        private readonly FirebaseService _firebase;
        private readonly HttpClient _http;

        public SiteController()
        {
            _firebase = new FirebaseService();
            _http = new HttpClient();
        }

        [HttpPost]
        public async Task<IActionResult> AddSite([FromBody] Site site)
        {
            CollectionReference sitesRef = _firebase.Db.Collection("sites");
            DocumentReference docRef = sitesRef.Document();
            site.Id = docRef.Id;
            site.Status = "Unknown";
            site.LastScan = DateTime.MinValue;
            site.CriticalAlert = false;

            await docRef.SetAsync(site);
            return Ok(site);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetSites(string userId)
        {
            CollectionReference sitesRef = _firebase.Db.Collection("sites");
            Query query = sitesRef.WhereEqualTo("UserId", userId);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            var sites = snapshot.Documents.ConvertAll(doc => doc.ConvertTo<Site>());
            return Ok(sites);
        }

        [HttpGet("scan/{siteId}")]
        public async Task<IActionResult> ScanSite(string siteId)
        {
            DocumentReference siteRef = _firebase.Db.Collection("sites").Document(siteId);
            DocumentSnapshot snapshot = await siteRef.GetSnapshotAsync();

            if (!snapshot.Exists)
                return NotFound();

            var site = snapshot.ConvertTo<Site>();

            // Scan básico: testa se o site responde HTTP
            try
            {
                var response = await _http.GetAsync(site.Url);
                site.Status = response.IsSuccessStatusCode ? "OK" : "Attention";
            }
            catch
            {
                site.Status = "Critical";
            }

            site.LastScan = DateTime.Now;
            site.CriticalAlert = site.Status == "Critical";

            await siteRef.SetAsync(site); // atualiza Firestore
            return Ok(site);
        }
    }
}