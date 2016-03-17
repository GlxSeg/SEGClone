using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEG.Web
{
    public partial class SiteMaster : MasterPage
    {
        private void Page_Init(object sender, EventArgs e)
        {
            //Security check here

            //Make sure there is a valid login:

            if (Session["UserId"] == null)
            {
                //no session, return to login
                Response.Redirect("pgLogin.aspx");
            }
            else
            {
                //Valid session, get user info and display it.
                //lblName.Text = Session["Name"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }

}