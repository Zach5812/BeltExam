#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BeltExam.Models;

public class User
{
    [Key]
    public int UserId { get; set; }


    [Required(ErrorMessage = "is required")]
    [MinLength(3, ErrorMessage = "Must be 3 characters")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "is required")]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }

    [Required(ErrorMessage = "is required")]
    [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z])(?=.*[-\#\$\!\@\.\%\&\*]).{8,}", ErrorMessage ="Password must be 8 characters long and include 1 number and 1 special character")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [NotMapped]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords need to match")]
    [Display(Name = "PW Confirm")]

    public string PasswordConfirm { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Coupon>? Coupons { get; set; } = new List<Coupon>();
    public List<CouponUser>? CouponUsers { get; set; } = new List<CouponUser>();
    public List<Expired>? Expireds { get; set; } = new List<Expired>();
}

public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Though we have Required as a validation, sometimes we make it here anyways
        // In which case we must first verify the value is not null before we proceed
        if (value == null)
        {
            // If it was, return the required error
            return new ValidationResult("Email is required!");
        }

        // This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        }
        else
        {
            // If no, proceed
            return ValidationResult.Success;
        }
    }
}




