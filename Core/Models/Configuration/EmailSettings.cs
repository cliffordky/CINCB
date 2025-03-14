namespace Core.Models.Configuration
{
    public class EmailSettings
    {
        public const string ConfigKey = nameof(EmailSettings);

        public string ApiKey { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        //public string TemplatePath { get; set; }
    }
}