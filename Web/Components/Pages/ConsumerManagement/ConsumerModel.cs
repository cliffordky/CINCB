namespace Web.Components.Pages.ConsumerManagement
{
    public class ConsumerModel
    {
        public int Id { get; set; }
        public Guid PublicKey { get; set; }
        public string ImageSlug { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
