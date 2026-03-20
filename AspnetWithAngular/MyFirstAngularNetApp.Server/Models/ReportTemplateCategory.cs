using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstAngularNetApp.Server.Models
{
    public partial class ReportTemplateCategory : GDCTEntityBase<int>
    {
        public int CombineReportId { get; set; }

        public int? CategoryId { get; set; }

        public int? AttributeId { get; set; }

        public int OrderNumber { get; set; }

    }
}
