namespace MyFirstAngularNetApp.Server.Models
{

    public partial class Region: GDCTEntityBase<int>
    {
        public string RegionName { get; set; } = null!;

        public virtual ICollection<Organization>? Organizations { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
