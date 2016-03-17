using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class RiskAnalysisType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        //public DateTime Created { get; set; }

        //public User CreatedBy { get; set; }

        //public DateTime LastModified { get; set; }

        //public User ModifiedBy { get; set; }
    }
}
