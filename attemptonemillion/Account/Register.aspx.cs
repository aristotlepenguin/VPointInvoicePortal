using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using attemptonemillion.Models;
using System.Data.SqlClient;
using System.Diagnostics;

namespace attemptonemillion.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            bool idPass = false;
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-GCAMI11Q\MSSQLSERVER01;Initial Catalog=TutorialDB;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM invoiceTable2 WHERE invoiceID = "+InvoiceID.Text+";";
            //
            SqlDataReader reader = cmd.ExecuteReader();
            string chaser = "";
            if (reader.Read()) {
                chaser = reader.GetInt32(0).ToString();
               
                idPass = true;
                reader.Close();
                cmd.CommandText = "UPDATE userids SET userEmail = \'" + Email.Text + "\' WHERE userID = " + chaser + ";";
                cmd.ExecuteNonQuery();
            }
            if (result.Succeeded && idPass)
            {
             
                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                signInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}