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

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    public int YearsOfExperience { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Experiences")]
    public virtual User User { get; set; } = null!;
}
