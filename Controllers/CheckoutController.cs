using ApexVolley.Data;
using ApexVolley.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System;
using System.Security.Claims;

[Authorize(Roles = "Tesserato")]
public class CheckoutController : Controller
{
    private readonly ApexVolleyContext _context;
    private readonly IConfiguration _config;

    public CheckoutController(ApexVolleyContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
        StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.CartItems.Include(c => c.Product)
            .Where(c => c.UserId == userId).ToListAsync();

        if (!cart.Any())
        {
            TempData["ErrorMessage"] = "Il carrello è vuoto!";
            return RedirectToAction("Index", "Products");
        }

        return View(new CheckoutViewModel { CartItems = cart });
    }

    [HttpPost]
    public async Task<IActionResult> CreateCheckoutSession(CheckoutViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var cart = await _context.CartItems.Include(c => c.Product)
            .Where(c => c.UserId == userId).ToListAsync();

        if (!cart.Any())
        {
            TempData["ErrorMessage"] = "Carrello vuoto!";
            return RedirectToAction("Index");
        }

        if (!ModelState.IsValid)
        {
            model.CartItems = cart;
            TempData["ErrorMessage"] = "Compila correttamente tutti i campi richiesti.";
            return View("Index", model);
        }

        // Creo ordine Pending
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.Now,
            TotalAmount = cart.Sum(c => c.Product.Price * c.Quantity),
            Status = "Pending",
            FullName = model.FullName,
            Address = model.Address,
            City = model.City,
            ZipCode = model.ZipCode,
            Country = model.Country,
            PhoneNumber = model.PhoneNumber,
            OrderItems = cart.Select(c => new OrderItem
            {
                ProductId = c.Product.Id,
                Quantity = c.Quantity,
                UnitPrice = c.Product.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Creo sessione Stripe
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = cart.Select(c => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(c.Product.Price * 100),
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = c.Product.Name
                    }
                },
                Quantity = c.Quantity
            }).ToList(),
            Mode = "payment",
            SuccessUrl = Url.Action("Success", "Checkout", new { orderId = order.Id }, Request.Scheme),
            CancelUrl = Url.Action("Index", "Checkout", null, Request.Scheme),
            ClientReferenceId = order.Id.ToString()
        };

        var service = new SessionService();
        var session = service.Create(options);

        if (session == null || string.IsNullOrEmpty(session.Url))
        {
            TempData["ErrorMessage"] = "Errore nella creazione del pagamento.";
            return RedirectToAction("Index");
        }

        order.StripeSessionId = session.Id;
        await _context.SaveChangesAsync();

        return Redirect(session.Url);
    }

    [HttpGet]
    public IActionResult Success(int orderId)
    {
        TempData["SuccessMessage"] = "Pagamento completato! Il tuo ordine verrà processato.";
        return RedirectToAction("Details", "Orders", new { id = orderId });
    }
}



[AllowAnonymous]
[Route("api/stripe/webhook")]
[ApiController]
public class StripeWebhookController : ControllerBase
{
    private readonly ApexVolleyContext _context;
    private readonly IConfiguration _config;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(ApexVolleyContext context, IConfiguration config, ILogger<StripeWebhookController> logger)
    {
        _context = context;
        _config = config;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        try
        {
            _logger.LogInformation("➡️ Webhook Stripe chiamato");
            var stripeEvent = EventUtility.ConstructEvent(json, signature, _config["Stripe:WebhookSecret"]);
            _logger.LogInformation("Evento Stripe ricevuto: {EventType}", stripeEvent.Type);

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                if (session == null)
                {
                    _logger.LogWarning("Session è null, impossibile elaborare l'evento");
                    return Ok();
                }

                _logger.LogInformation("Webhook ricevuto - SessionId: {SessionId}, PaymentStatus: {PaymentStatus}, ClientReferenceId: {ClientReferenceId}",
                    session.Id, session.PaymentStatus, session.ClientReferenceId);

                if (string.IsNullOrEmpty(session.ClientReferenceId))
                {
                    _logger.LogWarning("ClientReferenceId non impostato");
                    return Ok();
                }

                var orderId = int.Parse(session.ClientReferenceId);

                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.Status == "Pending");

                if (order != null && session.PaymentStatus == "paid")
                {
                    order.Status = "Pagato";
                    order.StripeSessionId = session.Id;

                    var cart = await _context.CartItems
                        .Where(c => c.UserId == order.UserId)
                        .ToListAsync();
                    _context.CartItems.RemoveRange(cart);

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Ordine {OrderId} aggiornato a Pagato e carrello rimosso", order.Id);
                }
                else
                {
                    _logger.LogWarning("Ordine non trovato o pagamento non completato");
                }
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Errore durante l'elaborazione del webhook Stripe");
            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Errore generico nel webhook Stripe");
            return BadRequest();
        }
    }
}
