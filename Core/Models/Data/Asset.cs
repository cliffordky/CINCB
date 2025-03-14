using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.Data
{
	[Table("Assets")]
	public class Asset : ControlFields
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public Guid PublicKey { get; set; }

		public int AssetTypeId { get; set; }
		public int OrganizationId { get; set; }
		public int? ParentAssetId { get; set; }

        //[Encrypted]
        [Column(TypeName = "varchar(250)")]
		public string Name { get; set; } = null!;

		[Column(TypeName = "varchar(MAX)")]
		public string Description { get; set; } = null!;

		public DateOnly? PurchaseDate { get; set; }

		[Column(TypeName = "varchar(50)")]
		public string Latitude { get; set; }

		[Column(TypeName = "varchar(50)")]
		public string Longitude { get; set; }

        public string RegisterNumber { get; set; }
        public string Notes { get; set; }
        public string ImageSlug { get; set; }
        public decimal? PurchaseValue { get; set; }
        public decimal? CurrentValue { get; set; }
    }
}