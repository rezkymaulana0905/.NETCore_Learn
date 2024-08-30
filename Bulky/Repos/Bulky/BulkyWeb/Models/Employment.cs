using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Employment
    {
        [Key]
        public int EMPID { get; set; }
        [Required]
        [MaxLength(255)]
        public string NAME { get; set; }
        [Required]
        [MaxLength(255)]
        public string STATUS { get; set; }
        [Required]
        [MaxLength(255)]
        public string JOBID { get; set; }
        [Required]
        public DateTime JOINDATE { get; set; }
    }
}
