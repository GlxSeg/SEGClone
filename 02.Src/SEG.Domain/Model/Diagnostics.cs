using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class Diagnostics
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public Guid AreaId { get; set; }

        public Guid DiagnosticsTypeId { get; set; }

        public int Index { get; set; }

        //public DateTime Created { get; set; }

        //public User CreatedBy { get; set; }

        //public DateTime LastModified { get; set; }

        //public User ModifiedBy { get; set; }
    }
}
