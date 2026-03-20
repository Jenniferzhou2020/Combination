using System.ComponentModel.DataAnnotations.Schema;

namespace MyFirstAngularNetApp.Server.Models;

public partial class NavMenu : GDCTEntityBase<int>
{
    public string? MenuItemName { get; set; }

    public string? PageName { get; set; }

    public string? NavUrl { get; set; }

    public int? SequenceNo { get; set; }

    public int? ParentId { get; set; }

    public string? NavMenuIcon { get; set; }

    public virtual ICollection<NavMenuRole>? NavMenuRoles { get; set; }
    [NotMapped]
    public virtual string? RoleNames { get; set; }
}
