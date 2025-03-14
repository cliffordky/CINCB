using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
	[Table("AssetAttributes")]
	public class AssetAttribute : ControlFields
	{
		[Key]	
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

        public int SourceId { get; set; }
        public int AssetId { get; set; }

		public int AttributeId { get; set; }

		public string GroupId { get; set; }

		[Column(TypeName = "varchar(MAX)")]
		public string Value { get; set; } = null!;

		public DateTime? DueDate { get; set; }
    }
}
