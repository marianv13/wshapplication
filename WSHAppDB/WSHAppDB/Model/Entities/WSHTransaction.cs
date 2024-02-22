#nullable disable
using System.ComponentModel.DataAnnotations;

namespace WSHAppDB.Model.Entities
{
    public partial class WSHTransaction : EntityBase
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Item { get; set; }

        [Required]
        public float Sum { get; set; }



    }
}