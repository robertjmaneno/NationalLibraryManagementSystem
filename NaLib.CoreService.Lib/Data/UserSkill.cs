using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

public partial class UserSkill
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("SkillID")]
    public int SkillId { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [ForeignKey("SkillId")]
    [InverseProperty("UserSkills")]
    public virtual Skill Skill { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserSkills")]
    public virtual User User { get; set; } = null!;
}
