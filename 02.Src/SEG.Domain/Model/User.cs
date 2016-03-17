using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public bool Active { get; set; }

        public bool IsExecutor { get; set; }

        public bool IsVerifier { get; set; }

        public bool IsApprover { get; set; }

        public bool IsAdmin { get; set; }        
    }
}
