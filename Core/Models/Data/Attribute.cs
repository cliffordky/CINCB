using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
	[Table("Attributes")]
	public  class Attribute : ControlFields
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int? SourceId { get; set; }
        public int DataTypeId { get; set; }
        public int AssetTypeId { get; set; }

		[Column(TypeName = "varchar(150)")]
		public string Name { get; set; }

		[Column(TypeName = "varchar(8000)")]
		public string Description { get; set; } = null!;

        public string NamespacePath { get; set; } = null!;

        public string CronExpression { get; set; } = null!;

        public bool IsIdentifier { get; set; }

        public bool IsUnique { get; set; }
    }
}
