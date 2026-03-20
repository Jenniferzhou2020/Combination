namespace MyFirstAngularNetApp.Server.Models
{

    public partial class Sector : GDCTEntityBase<int>
    {
        public string? SectorName { get; set; }

        public virtual ICollection<Organization>? Organizations { get; set; }

        public virtual ICollection<Template>? Templates { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}