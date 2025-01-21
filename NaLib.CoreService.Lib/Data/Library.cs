using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

[Table("Library")]
public partial class Library
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string LibraryName { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? LibraryLocation { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [InverseProperty("Library")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
