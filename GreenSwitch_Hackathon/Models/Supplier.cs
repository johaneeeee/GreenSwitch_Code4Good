namespace GreenSwitch.Models
{
    public enum VerificationStatus
    {
        Pending,
        Verified,
        Rejected,
        Suspended
    }

    public class Supplier
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<string> SDGs { get; set; } = new();
        public string Services { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string Contact { get; set; } = string.Empty;
        public VerificationStatus Status { get; set; }
        public DateTime VerifiedDate { get; set; }
        public List<string> Certifications { get; set; } = new();
        public string VerificationNotes { get; set; } = string.Empty;
        public int TrustScore { get; set; }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public bool Success { get; set; }
        public string Response { get; set; } = string.Empty;
        public List<Supplier>? Suppliers { get; set; }
        public string Intent { get; set; } = string.Empty;
    }
}