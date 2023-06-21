using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BeltExam.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers;
[SessionCheck]
public class CouponController : Controller
{
    private readonly ILogger<CouponController> _logger;
    private MyContext db;

    public CouponController(ILogger<CouponController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }


    [HttpGet("coupons")]
    public IActionResult Coupons()
    {
        List<Coupon> allCoupons = db.Coupons.Include(c=>c.User).Include(c=> c.CouponUsers).Include(c=>c.Expireds).ToList();
        
        if(allCoupons == null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View("Coupons", allCoupons);
    }


    [HttpGet("coupons/create")]
    public IActionResult AddCoupon()
    {
        return View("CouponForm");
    }


    [HttpPost("coupons/create")]
    public IActionResult CreateCoupon(Coupon newCoupon)
    {
        if (!ModelState.IsValid)
        {
            IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v=>v.Errors);
            foreach(ModelError error in errors)
            {
                Console.WriteLine(error.ErrorMessage.ToString());
            }
            Console.WriteLine(ModelState.IsValid);
            Console.WriteLine("Did Not Pass");
            return View("CouponForm");
        }
        db.Coupons.Add(newCoupon);

        db.SaveChanges();

        return RedirectToAction("Coupons");
}


[HttpPost("coupons/{couponId}/couponUser")]
public IActionResult CouponUserTo(int couponId)
{
    CouponUser? existingCouponUser = db.CouponUsers.FirstOrDefault(r=> r.UserId == HttpContext.Session.GetInt32("UUID")
    && r.CouponId == couponId);
    

    if(existingCouponUser == null)
    {
    CouponUser newCouponUser = new CouponUser()
    {
        CouponId = couponId,
        UserId = (int)HttpContext.Session.GetInt32("UUID")
    };

    db.CouponUsers.Add(newCouponUser);
    db.SaveChanges();
    }
    return RedirectToAction("Coupons");
}

[HttpPost("coupons/{couponId}/expired")]
public IActionResult MarkExpired(int couponId)
{
    Expired? existingExpired = db.Expireds.FirstOrDefault(e=> e.UserId == HttpContext.Session.GetInt32("UUID")
    && e.CouponId == couponId);
    

    if(existingExpired == null)
    {
    Expired newExpired = new Expired()
    {
        CouponId = couponId,
        UserId = (int)HttpContext.Session.GetInt32("UUID")
    };

    db.Expireds.Add(newExpired);
    db.SaveChanges();
    }
    return RedirectToAction("Coupons");
}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}



