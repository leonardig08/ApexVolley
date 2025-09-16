using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ApexVolley.Data;
using ApexVolley.Models;

[Authorize]
public class ProductsController : Controller
{
    private readonly ApexVolleyContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductsController(ApexVolleyContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.ToListAsync();
        return View(products);
    }

    [Authorize(Roles = "Admin,Staff")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Create(Product model, IFormFile MainImage)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            if (MainImage != null && MainImage.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await MainImage.CopyToAsync(stream);

                model.ImageUrl = "/uploads/" + fileName;
            }

            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Prodotto creato con successo!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Errore durante la creazione del prodotto: {ex.Message}");
            return View(model);
        }
    }

    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            TempData["ErrorMessage"] = "Prodotto non trovato.";
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Edit(int id, Product model, IFormFile MainImage)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;

            if (MainImage != null && MainImage.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads");
                Directory.CreateDirectory(uploads);

                var fileName = Guid.NewGuid() + Path.GetExtension(MainImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await MainImage.CopyToAsync(stream);

                product.ImageUrl = "/uploads/" + fileName;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Prodotto modificato con successo!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Errore durante la modifica del prodotto: {ex.Message}");
            return View(model);
        }
    }

    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Prodotto eliminato con successo!";
            }
            else
            {
                TempData["ErrorMessage"] = "Prodotto non trovato.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Errore durante l'eliminazione: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }
}
