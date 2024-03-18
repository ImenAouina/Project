#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chore_tracker.Models;

public class Job
{
    [Key]
    public int JobId { get; set; }

    [Required]
    [MinLength(4, ErrorMessage = "Title must be at least 4 characters")]

    public string Title { get; set; }

    [Required]
    [MinLength(10, ErrorMessage = "Title must be at least 10 characters")]
    public string Description { get; set; }

    [Required]
    public string Location { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation
    public User? Creator { get; set; }
    public List<UserJob> UserJobs { get; set; } = new List<UserJob>();
}
