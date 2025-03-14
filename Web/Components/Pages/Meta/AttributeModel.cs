namespace Web.Components.Pages.Meta
{
    public class AttributeModel
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public int DataTypeId { get;  set; }
        public string AssetType { get; internal set; }
        public bool IsUnique { get; internal set; }
        public string CronExpression { get; set; }
        public int ModifiedUser { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
