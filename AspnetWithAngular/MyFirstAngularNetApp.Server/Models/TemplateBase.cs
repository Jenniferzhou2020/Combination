namespace MyFirstAngularNetApp.Server.Models
{
    public class TemplateBase
    {
        public int CategoryId { get; set; }
        public int AttributeId { get; set; }
        public string? DataValue { get; set; }
        public string? CellLocation { get; set; }
        public string? Worksheet { get; set; }
    }
}
