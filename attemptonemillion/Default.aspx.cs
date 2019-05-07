using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;

namespace attemptonemillion 
{
    public partial class _Default : Page
    {
       
        /*
            pathMod: User pdfs are written to disk in a dedicated folder for that user. this string changes the filepath based on that user.
            context:The relative path to source.
            cmd, reader, outByte- Components for writing the masterpdf to its directory
        */
        public string foundIt= @"pdfpaths\default.pdf"; // file path string that the default page uses to display the master PDF. Defaults to an empty PDF.
        string pathMod = "";
        string context = HttpContext.Current.Server.MapPath(null)+"\\pdfpaths";
        protected void Page_Load(object sender, EventArgs e)
        {
            //var searchTerm = Request.QueryString["searchGenre"];
            int userid = 0;
            
            //if (!string.IsNullOrEmpty(searchTerm))
            //{
              //  userid = Int32.Parse(searchTerm);
                //Debug.WriteLine(userid);
            //}

            
            if (IsPostBack)
            {

            }//Connection string. 
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-GCAMI11Q\MSSQLSERVER01;Initial Catalog=TutorialDB;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT userID from userids WHERE userEmail=\'" + Context.User.Identity.GetUserName() + "\';";
            SqlDataReader reader = cmd.ExecuteReader(); //getting userid based on user.

            
            if (reader.Read()) {
                userid = reader.GetInt32(0);
                
            }
            reader.Close();
            if (userid != 0)
            {
                string path2 = context + "\\"+userid.ToString();//This creates a new directory for the user's current session. It's named after userID to keep each unique.
                
                Directory.CreateDirectory(path2);
                pathMod = "\\"+userid.ToString();
                foundIt = @"pdfpaths\"+userid.ToString()+@"\display.pdf";//Potential issue is if multiple users come in on the same account. Possible test for later.
            }

            cmd.CommandText = "SELECT pdfBlob FROM invoiceTable2 WHERE userID=" + userid + " AND isMaster=1;"; 
            reader = cmd.ExecuteReader();
            
            
            string path = context+pathMod+@"\display.pdf";//Default display name for masterpdfs.
            
            if (reader.Read())
            {
                byte[] outByte = (byte[])reader[0];
                
                File.WriteAllBytes(path, outByte); //Writes the master pdf to the directory (putting it on disk).
                Session["path"]= context + pathMod; //Writes the directory to the session, which will be used later in cleanup.
                //Session.Abandon();
            }
        }
       
        protected void DirectoryDelete(object sender, EventArgs e) {//Unused, deleting directories is now in Global.asax
            
            if (pathMod != "" && File.Exists(context + pathMod + @"\display.pdf")) {
                
                Directory.Delete(context + pathMod, true);

            }
        }
        //starter, strip, text, idNo: PDF variable, text stripper, full PDF text, invoice ID, respectively.
        protected void Button1_ServerClick(object sender, EventArgs e)//Download button function.
        { //Debug.WriteLine(Value1.Text);
            int pageNum = 0;
            Int32.TryParse(Value1.Text, out pageNum);//Int32 has to be run twice to get its value for the next if statement
                                                     // pageNum = Int32.Parse(Value1.Text);//Convert integer to text
            PDDocument starter;
            starter = PDDocument.load(context + pathMod + @"\display.pdf");
            if (pageNum > 0 && pageNum<=starter.getNumberOfPages() && pathMod!="" && Int32.TryParse(Value1.Text, out pageNum))
            {//check if field is a number, and if that number is in page range
                PDFTextStripper strip = new PDFTextStripper();
                strip.setStartPage(pageNum);
                strip.setEndPage(pageNum);//only extracting the specified page
                string text = strip.getText(starter);
                string markerS = text.Substring(text.IndexOf("Invoice #") + 11, 6);
                int idNo = Int32.Parse(markerS);
                downloadItem(idNo);//The page is needed only for the invoice ID. The actual download takes place in this function.
            }
            starter.close();
        }
        protected void Button2_ServerClick(object sender, EventArgs e)
        {
/*
           var userid = Int32.Parse(TextBox2.Text);
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-GCAMI11Q\MSSQLSERVER01;Initial Catalog=TutorialDB;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT pdfBlob FROM invoiceTable2 WHERE userID= " + userid + " AND isMaster=1;";
            //cmd.CommandText = cmd.CommandText = "SELECT pdfBlob FROM invoiceTable2 WHERE userID= " + userid.ToString() + " AND isMaster=1;";
            string path = @"C:\Users\danie\source\repos\attemptonemillion\attemptonemillion\pdfpaths\display.pdf";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                byte[] outByte = (byte[])reader[0];
                File.WriteAllBytes(path, outByte);
            }
            reader.Close();*/
        }
        protected void downloadItem(int idNo)
        {
            //conn, cmd, reader, outByte: Connection string, SQL command, data reader, and output stream respectively.
            SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-GCAMI11Q\MSSQLSERVER01;Initial Catalog=TutorialDB;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT pdfBlob FROM invoiceTable2 WHERE invoiceID= " + idNo + " AND isMaster=0;";//Extract the single invoice from the database
            //cmd.CommandText = cmd.CommandText = "SELECT pdfBlob FROM invoiceTable2 WHERE userID= " + userid.ToString() + " AND isMaster=1;";
           // string path = @"C:\Users\danie\source\repos\WebApplication4\WebApplication4\Properties\download.pdf";
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                byte[] outByte = (byte[])reader[0];
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=" + idNo.ToString() + ".pdf");
                Response.BufferOutput = true;
                Response.OutputStream.Write(outByte, 0, outByte.Length);//just a byte stream output
                Response.End();
            }
        }
    }
}