using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace ApexVolley.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Register
        [HttpGet]
        public IActionResult Index() => View();

        // POST: /Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Assicura che il ruolo Tesserato esista
            if (!await _roleManager.RoleExistsAsync("Tesserato"))
                await _roleManager.CreateAsync(new IdentityRole("Tesserato"));

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true, // permette login immediato
                Nome = model.Nome,
                Cognome = model.Cognome,
                RuoloCompleto = "Tesserato"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Aggiunge l'utente al ruolo Tesserato
                await _userManager.AddToRoleAsync(user, "Tesserato");

                // Login automatico
                await _signInManager.SignInAsync(user, isPersistent: false);

                TempData["SuccessMessage"] = "Registrazione completata con successo!";
                return RedirectToAction("Index", "Home");
            }

            // Aggiunge gli errori di Identity al ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}
