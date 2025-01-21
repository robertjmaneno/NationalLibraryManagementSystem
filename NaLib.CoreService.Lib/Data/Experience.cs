using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Table("Experience")]
public partial class Experience
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string OrganizationName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string JobTitle { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Location { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Experiences")]
    public virtual User User { get; set; } = null!;
}
