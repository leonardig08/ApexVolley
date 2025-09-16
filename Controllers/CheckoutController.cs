using ApexVolley.Data;
using ApexVolley.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Tesserato")]
public class CheckoutController : Controller
{
    private readonly ApexVolleyContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutController(ApexVolleyContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
        decimal total = cart.Sum(c => c.Product.Price * c.Quantity);
        ViewData["Total"] = total;

        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
        if (!cart.Any())
        {
            TempData["ErrorMessage"] = "Il carrello è vuoto!";
            return RedirectToAction("Index", "Products");
        }

        if (!ModelState.IsValid)
        {
            decimal total = cart.Sum(c => c.Product.Price * c.Quantity);
            ViewData["Total"] = total;
            return View(cart);
        }

        // Crea ordine
        var order = new Order
        {
            UserId = _httpContextAccessor.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value,
            CreatedAt = DateTime.Now,
            TotalAmount = cart.Sum(c => c.Product.Price * c.Quantity),
            Status = "In attesa",
            OrderItems = cart.Select(c => new OrderItem
            {
                ProductId = c.Product.Id,
                Quantity = c.Quantity,
                UnitPrice = c.Product.Price
            }).ToList()
        };

        // Aggiungi dati spedizione
        order.FullName = model.FullName;
        order.Address = model.Address;
        order.City = model.City;
        order.ZipCode = model.ZipCode;
        order.Country = model.Country;
        order.PhoneNumber = model.PhoneNumber;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Svuota carrello
        HttpContext.Session.Remove("Cart");

        TempData["SuccessMessage"] = "Ordine creato con successo!";
        return RedirectToAction("Index", "Orders");
    }
}
