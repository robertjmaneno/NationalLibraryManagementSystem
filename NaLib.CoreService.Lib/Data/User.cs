using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Table("User")]
[Index("Username", Name = "UQ__User__536C85E44878B525", IsUnique = true)]
[Index("Email", Name = "UQ__User__A9D10534DAE5DAE0", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool IsPasswordExpired { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastPasswordChangeDate { get; set; }

    public bool IsActive { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [Column("LibraryID")]
    public int LibraryId { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    [InverseProperty("User")]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    [InverseProperty("LendedByNavigation")]
    public virtual ICollection<LendingTransaction> LendingTransactions { get; set; } = new List<LendingTransaction>();

    [ForeignKey("LibraryId")]
    [InverseProperty("Users")]
    public virtual Library Library { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<UserSkill> UserSkills { get; set; } = new List<UserSkill>();
}
