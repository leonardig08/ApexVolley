using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();

        var model = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserRolesViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles
            });
        }

        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        ViewBag.AllRoles = allRoles;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddRole(string userId, string roleName)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
        {
            return BadRequest();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            return BadRequest("Ruolo inesistente");
        }

        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveRole(string userId, string roleName)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
        {
            return BadRequest();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CreateUser(
    string userName,
    string email,
    string password,
    string nome,
    string cognome,
    string ruoloCompleto)
    {
        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ModelState.AddModelError("", "Compila tutti i campi obbligatori");
            return RedirectToAction(nameof(Index));
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            Nome = nome,
            Cognome = cognome,
            RuoloCompleto = ruoloCompleto,
            DataRegistrazione = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            TempData["Errors"] = string.Join("; ", result.Errors.Select(e => e.Description));
        }
        else
        {
            TempData["Success"] = "Utente creato correttamente";
        }

        return RedirectToAction(nameof(Index));
    }

}

public class UserRolesViewModel
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public IList<string> Roles { get; set; }
}
