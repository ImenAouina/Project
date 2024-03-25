using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chore_tracker.Models;

namespace chore_tracker.Controllers;

public class JobController : Controller
{
   
    private Context _context;

    public JobController(Context context)
    {
        _context = context;
    }

    // add Job 

    [HttpGet("addJob")]
    public IActionResult AddJob()
    {
        if (!IsUserLoggedIn()) return RedirectToIndex();
        return View();
    }

    [HttpPost("CreateJob")]
    public IActionResult CreateJob(Job NewJob)
    {
         System.Console.WriteLine(NewJob.UserId);
        if (ModelState.IsValid)
        {
        _context.Add(NewJob);
        _context.SaveChanges();
         return RedirectToAction("Index","Home");
        }

        return View("AddJob");
    }

    // update Job 

    [HttpGet("edit/{JobId}")]
    public IActionResult EditJob(int JobId)
    {
      if (!IsUserLoggedIn()) return RedirectToIndex();
      var JobToUpdate = _context.Jobs.FirstOrDefault(j => j.JobId == JobId);
      var User = HttpContext.Session.GetInt32("UserId");
      ViewBag.UserId = User;
      return View(JobToUpdate);
    }

    [HttpPost("UpdateJob")]
    public IActionResult UpdateJob(Job EditedJob)
    {
    
        if (ModelState.IsValid)
        {
            Job oldJob = _context.Jobs.FirstOrDefault(j => j.JobId == EditedJob.JobId);
            oldJob.Title = EditedJob.Title;
            oldJob.Description = EditedJob.Description;
            oldJob.Location = EditedJob.Location;           
            oldJob.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        return View("EditJob", EditedJob.JobId);
    }


//delete Job
    
    [HttpGet("delete/{JobId}")]
    public IActionResult DeleteJob(int JobId)
    {
      if (!IsUserLoggedIn()) return RedirectToIndex();
       var JobToDelete = _context.Jobs.FirstOrDefault(j => j.JobId == JobId);
        if (JobToDelete != null)
        {
            List<UserJob> relatedUserJob = _context.UserJobs.Where(u => u.JobId == JobId).ToList();
            foreach (UserJob a in relatedUserJob.ToList())
            {

                    _context.UserJobs.Remove(a);
                    _context.SaveChanges();

            }
            _context.Jobs.Remove(JobToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    return RedirectToAction("Index","Home");
    }

// display Job

    [HttpGet("view/{JobId}")]
    public IActionResult showOne(int JobId)
    {
    if (!IsUserLoggedIn()) return RedirectToIndex();
    Job selectedJob = _context.Jobs
                                  .Include(e => e.Creator)
                                  .Include(e => e.UserJobs)
                                  .FirstOrDefault(j => j.JobId == JobId);              
    // current user
    var CurrentUser = HttpContext.Session.GetInt32("UserId");
    ViewBag.CurrentUser = CurrentUser;  

    return View(selectedJob);
    }

//Add to my jobs
    [HttpGet("AddToJobList")]
    public IActionResult AddToJobList(int JobId)
    {
    var CurrentUser = HttpContext.Session.GetInt32("UserId") ?? 0;
    var newJob = new UserJob
        {
            UserId = CurrentUser,
            JobId = JobId
        };
    _context.UserJobs.Add(newJob);
    _context.SaveChanges();
    return RedirectToAction("Index","Home");
    }
    
 //---------------------------------IsUserLoggedIn method
    private bool IsUserLoggedIn()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }

    private IActionResult RedirectToIndex()
        {
            ModelState.AddModelError("LoginPassword", "You should connect!");
            return RedirectToAction("Index", "User");
        }

}





