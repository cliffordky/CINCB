namespace Web.Components.Pages.Meta
{
    public class AssetTypeModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
