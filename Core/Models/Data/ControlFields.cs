using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Data
{
    public class ControlFields
    {
        [Required]
        [DefaultValue(9999)]
        public int Sequence { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool AllowUserDelete { get; set; }

        [Required]
        [DefaultValue(0)]
        public int StatusId { get; set; }

        [Required]      
        public DateTime ModifiedDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        [DefaultValue(0)]
        public int ModifiedUser { get; set; }
    }
}
