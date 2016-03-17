using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain
{
    public class SEGRepository
    {
        private string connStr;

        public SEGRepository(string aconnStr)
        {
            connStr = aconnStr;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SEGContext>());
        }

        public SEGContext GetContext()
        {
            return new SEGContext(connStr);
        }
    }
}
