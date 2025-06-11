using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My.Database.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [EmailAddress]
    [StringLength(32)]
    public required string Email { get; set; } // Mail address and username.

    [StringLength(32)]
    public required string FirstName { get; set; }

    [StringLength(32)]
    public required string LastName { get; set; }

    [Column("RoleId")]
    public UserRole Role { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Sub Models

    [InverseProperty(nameof(UserActivation.User))]
    public UserActivation Activation { get; set; } = null!;
}

public class UserActivation
{
    [Key, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public string? ActivationToken { get; set; }

    public DateTime? ActivationTokenExpiration { get; set; }

    public User User { get; set; } = null!;
}

public enum UserRole
{
    Admin,
    Manager,
    User
}