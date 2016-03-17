using SEG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Domain.Helpers
{
    static public class AccountHelper
    {
        static public User CheckLogin(SEGRepository segRep, string user)
        {
            User u = null;
            using (SEGContext seg = segRep.GetContext())
            {
                u = seg.Users.FirstOrDefault(x => x.Name.Trim().ToUpper() == user.Trim().ToUpper());
            }
            return u;
        }
    }
}
