namespace MyFirstAngularNetApp.Server.Models
{
    public class TemplateSubmission : TemplateBase
    {
        public string? CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string? Status { get; set; }
    }
}

