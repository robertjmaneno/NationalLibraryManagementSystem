using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Keyless]
public partial class VwUserDetail
{
    public int UserId { get; set; }

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
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool IsActive { get; set; }

    public DateOnly UserCreatedAt { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? LibraryName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? RoleName { get; set; }

    public int? YearsOfExperience { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string? GradeName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? QualificationName { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? SkillName { get; set; }
}
