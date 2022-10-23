using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EBSApi.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string? fullName { get; set; }
        public bool type { get; set; }
        public double balance { get; set; }
        public bool active { get; set; }
    }
}
