using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class AssetAudit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public User User { get; set; }

        public string Event { get; set; }

        public string Value { get; set; }

        public DateTime EventTime { get; set; }
    }
}
