using System;

namespace IronSec.Models
{
    public class Site
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Url { get; set; }
        public string Status { get; set; } = "Unknown";
        public DateTime LastScan { get; set; } = DateTime.MinValue;
        public bool CriticalAlert { get; set; } = false;
    }
}