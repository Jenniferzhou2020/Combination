using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ReportTemplateData : GDCTEntityBase<int>
    {
        public int? TemplateId { get; set; }

        public int? AttributeId { get; set; }

        public bool SubTotalFlag { get; set; }

        public bool IsCalculationData { get; set; }

        public int OrderNumber { get; set; }
    }
}
