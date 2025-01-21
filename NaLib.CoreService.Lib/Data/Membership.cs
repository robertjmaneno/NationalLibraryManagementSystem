using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Index("EnrollmentDate", Name = "IX_Memberships_EnrollmentDate")]
[Index("OverDueCount", Name = "IX_Memberships_OverDueCount")]
[Index("Status", Name = "IX_Memberships_Status")]
[Index("MemberId", Name = "UQ__Membersh__0CF04B1908625B97", IsUnique = true)]
public partial class Membership
{
    [Key]
    [StringLength(255)]
    public string MembershipId { get; set; } = null!;

    public int MemberId { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    [StringLength(100)]
    public string Status { get; set; } = null!;

    public int OverDueCount { get; set; }

    [StringLength(255)]
    public string? PreferredGenres { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("Membership")]
    public virtual Member Member { get; set; } = null!;
}
