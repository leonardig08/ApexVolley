using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Data;
using ApexVolley.Models;
using System.Security.Claims;

[Authorize]
public class CartController : Controller
{
    private readonly ApexVolleyContext _context;

    public CartController(ApexVolleyContext context)
    {
        _context = context;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

    private bool IsTesserato() => User.IsInRole("Tesserato"); // esempio claim Tesserato

    // Mostra carrello
    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var items = await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        var total = items.Sum(i => i.Product.Price * i.Quantity);

        ViewData["Total"] = total;
        return View(items);
    }

    // Aggiungi prodotto al carrello
    [HttpPost]
    public async Task<IActionResult> Add(int productId)
    {
        if (!IsTesserato())
        {
            TempData["ErrorMessage"] = "Solo utenti tesserati possono aggiungere prodotti al carrello.";
            return RedirectToAction("Index", "Products");
        }

        var userId = GetUserId();

        var existing = await _context.CartItems
            .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            var item = new CartItem
            {
                UserId = userId,
                ProductId = productId,
                Quantity = 1
            };
            _context.CartItems.Add(item);
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Prodotto aggiunto al carrello!";
        return RedirectToAction("Index", "Products");
    }

    // Rimuovi prodotto
    [HttpPost]
    public async Task<IActionResult> Remove(int id)
    {
        var item = await _context.CartItems.FindAsync(id);
        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Prodotto rimosso dal carrello.";
        }
        return RedirectToAction("Index");
    }

    // Aggiorna quantità
    [HttpPost]
    public async Task<IActionResult> Update(int id, int quantity)
    {
        var item = await _context.CartItems.FindAsync(id);
        if (item != null)
        {
            if (quantity < 1) quantity = 1;
            item.Quantity = quantity;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
