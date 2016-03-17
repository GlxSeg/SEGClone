using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class AssetInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public Guid Id { get; set; }

        public Guid AssetId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        //public DateTime Created { get; set; }

        //public User CreatedBy { get; set; }

        //public DateTime LastModified { get; set; }

        //public User ModifiedBy { get; set; }
    }
}
