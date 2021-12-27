using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreTest.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string OrderNo { get; set; }

        [Required,DefaultValue("0")]
        public decimal Amount { get; set; }

        [Required, DefaultValue("datetime('now')")]
        public DateTime CreateTime { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }
    }
}
