using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Index("CreatedAt", Name = "IX_Members_CreatedAt")]
[Index("FirstName", Name = "IX_Members_FirstName")]
[Index("LastName", Name = "IX_Members_LastName")]
[Index("Email", Name = "UQ__Members__A9D1053440311B44", IsUnique = true)]
[Index("Email", Name = "UX_Members_Email", IsUnique = true)]
public partial class Member
{
    [Key]
    public int MemberId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    [StringLength(255)]
    public string? PostalAddress { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [StringLength(255)]
    public string? PhysicalAddress { get; set; }

    [InverseProperty("Member")]
    public virtual ICollection<LendingTransaction> LendingTransactions { get; set; } = new List<LendingTransaction>();

    [InverseProperty("Member")]
    public virtual Membership? Membership { get; set; }
}
