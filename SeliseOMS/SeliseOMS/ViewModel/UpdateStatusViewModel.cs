using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeliseOMS.ViewModel
{
    public class UpdateStatusViewModel
    {
        [DisplayName("Order Status")]
        [Required]
        [MaxLength(50)]
        public string OrderStatus { get; set; } = null!;

        [Required]
        public int OrderId { get; set; }
    }
}
