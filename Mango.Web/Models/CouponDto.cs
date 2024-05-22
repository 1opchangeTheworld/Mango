using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        [Display(Name = "Coupon Code")]
        public string CouponCode { get; set; }
		[Display(Name = "Discount Amount")]
		public double DiscountAmount { get; set; }
		[Display(Name = "Min Amount")]
		public int MinAmount { get; set; }
    }
}
