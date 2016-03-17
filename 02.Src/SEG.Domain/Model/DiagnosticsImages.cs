using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class DiagnosticsImages
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid DiagnosticID { get; set; }

        public Guid ImageId { get; set; }

        public int Order { get; set; }
    }
}
