namespace MyFirstAngularNetApp.Server.Models;

public partial class Organization : GDCTEntityBase<int>
{
    public string? OrganizationName { get; set; }

    public int FacilityNo { get; set; }

    public string? IFIS { get; set; }

    public string? LHINName { get; set; }

    public string? OrganizationLegalName { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? City { get; set; }

    public string? PostalCode { get; set; }

    public string? CFOName { get; set; }

    public string? CFOPosition { get; set; }

    public string? CFOTele { get; set; }

    public string? CFOEmail { get; set; }

    public string? FinanceName { get; set; }

    public string? FinancePosition { get; set; }

    public string? FinanceTele { get; set; }

    public string? FinanceEmail { get; set; }

    public string? BoardChairName { get; set; }

    public string? BoardChairPosition { get; set; }

    public string? BoardChairTele { get; set; }

    public string? BoardChairEmail { get; set; }

    public string? BoardCoChairName { get; set; }

    public string? BoardCoChairPosition { get; set; }

    public string? BoardCoChairTele { get; set; }

    public string? BoardCoChairEmail { get; set; }

    public int? RegionId { get; set; }

    public int? SectorId { get; set; }

    public virtual Region? Region { get; set; }

    public virtual Sector? Sector { get; set; }

    public virtual ICollection<Submission>? Submissions { get; set; }

    public virtual ICollection<PublishLog>? PublishLogs { get; set; }
    public virtual ICollection<Gdctdatum>? Gdctdata { get; set; }
    public virtual ICollection<GdctdataCalculation> GdctdataCalculations { get; set; } = new List<GdctdataCalculation>();
}
