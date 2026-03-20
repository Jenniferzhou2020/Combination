namespace MyFirstAngularNetApp.Server.Models
{
    public partial class TemplateStatus: GDCTEntityBase<int>
    {
        public string? StatusName { get; set; }
        public virtual ICollection<Template>? Templates { get; set; }
        
    }
}
