using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chore_tracker.Models;

namespace chore_tracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Context _context;

    public HomeController(ILogger<HomeController> logger, Context context)
    {
        _logger = logger;
        _context = context;
    }

    //Dashboard//
    //[HttpGet("dashboard")]
    public IActionResult Index()
    {
        if (!HttpContext.Session.GetInt32("UserId").HasValue)
        {
            return RedirectToAction("Index", "Auth");
        }
        List<Job> AllJobs = _context.Jobs
        .Include(j => j.Creator)
        .Include(j=> j.UserJobs)
        .ToList();
        return View(AllJobs);
    }
    //Delete Job //
    public IActionResult DeleteJob(int jobId)
    {
        Job? JobToRemove = _context.Jobs.SingleOrDefault(j=> j.JobId == jobId);
        _context.Jobs.Remove(JobToRemove);
        _context.SaveChanges();
        return RedirectToAction("Index","Home");
    }
    //Add to my Jobs
     public IActionResult AddToMyJobs(int jobId)
    {
         if (!HttpContext.Session.GetInt32("UserId").HasValue)
        {
            return RedirectToAction("Index", "Auth");
        }
        UserJob newUserJob = new UserJob
        {
            UserId = (int)HttpContext.Session.GetInt32("UserId"),
            JobId = jobId
        };
            _context.UserJobs.Add(newUserJob);
            _context.SaveChanges();
            return RedirectToAction("Index");   
    }

    //Add Job 
    [HttpGet("addJob")]
    public IActionResult AddJob()
    {
        if (!HttpContext.Session.GetInt32("UserId").HasValue)
        {
            return RedirectToAction("Index", "Auth");
        }
        return View("AddJob");
    }

    [HttpPost]
    public IActionResult CreateJob(Job newJob)
    {
        if (!HttpContext.Session.GetInt32("UserId").HasValue)
        {
            return RedirectToAction("Index", "Auth");
        }
        if (ModelState.IsValid)
        {
            _context.Jobs.Add(newJob);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        return View("AddJob");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
