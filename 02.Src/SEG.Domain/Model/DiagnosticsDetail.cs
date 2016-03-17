using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class DiagnosticsDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid DiagnosticsId { get; set; }

        public Guid ItemId { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public Guid ImageId { get; set; }

        public Guid ExecutorId { get; set; }

        public Guid VerifierId { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime VerifyDate { get; set; }

        //public DateTime Created { get; set; }

        //public User CreatedBy { get; set; }

        //public DateTime LastModified { get; set; }

        //public User ModifiedBy { get; set; }
    }
}
