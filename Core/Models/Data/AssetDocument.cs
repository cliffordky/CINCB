using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
    [Table("AssetDocuments")]
    public class AssetDocument : ControlFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid PublicKey { get; set; }

        public int AssetId { get; set; }
        public string Name { get; set; } = null!;

        public string FileName { get; set; }
    }
}
