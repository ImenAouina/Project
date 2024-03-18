#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;

namespace chore_tracker.Models;

public class LoginUser
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string LoginEmail { get; set; }

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string LoginPassword { get; set; }
}
