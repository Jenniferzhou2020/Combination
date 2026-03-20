namespace MyFirstAngularNetApp.Server.Models
{
    public partial class LookupMapping: GDCTEntityBase<int>
    {
        public string? VariableName { get; set; } 

        public string? MappedField { get; set; } 

        public string? Worksheet { get; set; } 

        public int TemplateId { get; set; }

        public virtual Template? Template { get; set; } 
    }
}
