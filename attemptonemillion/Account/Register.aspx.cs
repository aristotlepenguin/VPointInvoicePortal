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
using System.Configuration;

namespace attemptonemillion.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (checkAdmin(Context.User.Identity.GetUserName()))
            {

            }
            else {
                Response.Redirect("/Default.aspx");
            }
        }
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            /*VARIABLE TABLE
             
             idPass- Invoice ID validiy flag
             manager, signInManager, user- VS default checks for valid email/password information
             conn, cmd, reader- objects to extract the ID from database
             chaser- chosen userID as a string
             SUMMARY: The function checks the database for the typed invoiceID, finds the userID attached to it, and updates the userID profile
             to correspond to the newly registered user.*/
            bool idPass = false;
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID FROM invoiceTable2 WHERE invoiceID = "+InvoiceID.Text+";";
            //
            SqlDataReader reader = cmd.ExecuteReader();
            string chaser = "";
            if (reader.Read()) {
                chaser = reader.GetInt32(0).ToString();
               
                idPass = true;//additional flag for data entry, checks if the invoice ID is an existing valid entry in the table.
                reader.Close();
                cmd.CommandText = "UPDATE userids SET userEmail = \'" + Email.Text + "\' WHERE userID = " + chaser + ";";
                cmd.ExecuteNonQuery();
                if (CheckBox1.Checked)
                {
                    cmd.CommandText = "UPDATE userids SET isAdmin = 1 WHERE userID = " + chaser + ";";
                    cmd.ExecuteNonQuery();
                }
                else {
                    cmd.CommandText = "UPDATE userids SET isAdmin = 0 WHERE userID = " + chaser + ";";
                    cmd.ExecuteNonQuery();
                }
            }
            if (result.Succeeded && idPass)//If so, register.
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

        public static bool checkAdmin(string email) {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT isAdmin FROM userids WHERE userEmail=\'"+email+"\';";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read()) {
                if (reader.GetBoolean(0)){
                    conn.Close();
                    reader.Close();
                    return true;
                }
            }

            reader.Close();
            conn.Close();

            return false;
        }
    }
}