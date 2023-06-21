#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BeltExam.Models;

public class Coupon
{
    [Key]
    public int CouponId { get; set; }
    [Required]
    [Display(Name ="Coupon Code")]
    public string? CouponCode { get; set; }
    [Required]
    [Display(Name ="Website Applicable On")]
    public string? Website { get; set; }
    [Required]
    [MinLength(10, ErrorMessage ="Must be 10 characters")]
    public string? Description { get; set; }

    [Required]
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<CouponUser>? CouponUsers { get; set; }
    public List<Expired>? Expireds { get; set; }
    public User? User { get; set; }
}
