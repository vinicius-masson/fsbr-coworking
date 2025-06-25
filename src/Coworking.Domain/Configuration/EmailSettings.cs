namespace Coworking.Domain.Configuration
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public string DestinationEmail { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }
}
