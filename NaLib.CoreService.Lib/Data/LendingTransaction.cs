using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Table("LendingTransaction")]
public partial class LendingTransaction
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public DateOnly LendingDate { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? ResourceCondition { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string LendingStatus { get; set; } = null!;

    public bool IsAlertIssued { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [Column("MemberID")]
    public int MemberId { get; set; }

    [Column("ResourceID")]
    public int ResourceId { get; set; }

    public int? LendedBy { get; set; }

    [ForeignKey("LendedBy")]
    [InverseProperty("LendingTransactions")]
    public virtual User? LendedByNavigation { get; set; }

    [ForeignKey("MemberId")]
    [InverseProperty("LendingTransactions")]
    public virtual Member Member { get; set; } = null!;
}
