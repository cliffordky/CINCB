using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Data
{
    [Table("CohortFilters")]
    public class CohortFilter : ControlFields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CohortId { get; set; }

        public int AttributeId { get; set; }
        public int QueryTypeId { get; set; }
        public string Value { get; set; }

        public string ConjunctiveOperator { get; set; }
    }
}
