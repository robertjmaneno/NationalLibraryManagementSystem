using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NaLib.CoreService.Lib.Data;

public partial class RolePermission
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RoleID")]
    public int RoleId { get; set; }

    [Column("PermissionsID")]
    public int PermissionsId { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    [ForeignKey("PermissionsId")]
    [InverseProperty("RolePermissions")]
    public virtual Permission Permissions { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("RolePermissions")]
    public virtual Role Role { get; set; } = null!;
}
