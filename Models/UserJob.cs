#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chore_tracker.Models;

public class UserJob
{
    [Key]
    public int UserJobId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int JobId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Navigation
    public User? User { get; set; }
    public Job? Job { get; set; }
}
