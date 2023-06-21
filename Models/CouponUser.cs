#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BeltExam.Models;

public class CouponUser
{
    [Key]
    public int CouponUserId { get; set; }
    public int CouponId { get; set; }
    public int UserId { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }
    public Coupon? Coupon { get; set; }
}