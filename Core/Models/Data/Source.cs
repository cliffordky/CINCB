using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
	[Table("Sources")]
	public class Source : ControlFields
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Column(TypeName = "varchar(150)")]
		public string Name { get; set; }

		[Column(TypeName = "varchar(8000)")]
		public string Description { get; set; } = null!;
	}
}
