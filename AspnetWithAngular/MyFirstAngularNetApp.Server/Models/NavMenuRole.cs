namespace MyFirstAngularNetApp.Server.Models;

public partial class NavMenuRole : GDCTEntityBase<int>
{
    public int NavMenuId { get; set; }

    public int? UserRoleId { get; set; }

    public virtual NavMenu? NavMenu { get; set; }

    public virtual UserRole? UserRole { get; set; }
}
