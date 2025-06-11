using System.ComponentModel.DataAnnotations;
using My.Database.Models;

namespace My.App.Dtos;

public class UserDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [StringLength(32, ErrorMessage = "Email address can't be longer than 32 characters")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(32, ErrorMessage = "First name can't be longer than 32 characters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(32, ErrorMessage = "Last name can't be longer than 32 characters")]
    public required string LastName { get; set; }
    public UserRole Role { get; set; }
}