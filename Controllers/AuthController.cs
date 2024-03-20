using System.Diagnostics;
using chore_tracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace chore_tracker.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly Context _context;

    public AuthController(ILogger<AuthController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("/auth")]
    public IActionResult Index()
    {
        return View();
    }

    // Processing Register
    [HttpPost("/register/submit")]
    public IActionResult Register(User newUser)
    {
        // Model-level Validation
        if (!ModelState.IsValid)
            return View("Index");

        // Email Uniqueness
        if (_context.Users.Any(u => u.Email == newUser.Email))
        {
            ModelState.AddModelError("Email", "Email already in use.");
            return View("Index");
        }

        // Password Hashing
        PasswordHasher<User> Hasher = new PasswordHasher<User>();
        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

        // Save User
        _context.Users.Add(newUser);
        _context.SaveChanges();

        HttpContext.Session.SetInt32("UserId", newUser.UserId);
        HttpContext.Session.SetString("UserName", newUser.FirstName);
        return RedirectToAction("Index", "Home");
    }

    // Processing Login
    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        // Model-level Validation
        if (!ModelState.IsValid)
            return View("Index");

        // Creditials Validation
        // Email
        User? userInDb = _context.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
        if (userInDb == null)
        {
            ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
            return View("Index");
        }

        // Password
        PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
        var isValidPassword = Hasher.VerifyHashedPassword(
            loginUser,
            userInDb.Password,
            loginUser.LoginPassword
        );
        if (isValidPassword == 0)
        {
            ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
            return View("Index");
        }

        HttpContext.Session.SetInt32("UserId", userInDb.UserId);
        HttpContext.Session.SetString("UserName", userInDb.FirstName);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
