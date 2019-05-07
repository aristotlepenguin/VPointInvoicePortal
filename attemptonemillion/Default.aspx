
<%--
Page: Default.aspx 
       Purpose: The main page. Navigate to this site and this is the default page you'll find.
       
    Inputs and Outputs:
        div class "reader":
            Contains the embed that displays the PDF.
        btn:
            Download button.
        Value1:
            Text box. Enter the invoice you want to download by page number, and the system will download that invoice.
    Notes:
            -You'll notice there's some inline CSS here. The SASS files that contained default settings could not reliably be changed. 
            Any changes to the site's visual design will function simplest here.
            -Navbar edits happen in Site.Master
    --%>
<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="attemptonemillion._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div class ="reader"><!--style="position: relative; left: 300px"-->
            <embed src="<%= foundIt %>" type="application/pdf" width="650px" height="600px" style="padding-top:25px" />
        </div>
    <asp:Button ID="btn" text="Download" runat="server" OnClick="Button1_ServerClick" style="box-shadow: none;text-shadow: 0 1px 0 rgba(0, 0, 0, 0.1);border: 1px solid transparent;padding: 8px 12px;background:linear-gradient(#54b4eb, #2fa4e7 60%, #1d9ce5);border-radius:4px;" />
    <asp:TextBox ID="Value1" Columns="2" MaxLength="5" Text="Page#" runat="server" style="padding: 3px 12px;"/>  
    

   <%-- <div class="jumbotron">
        <h1>ASP.NET</h1>
        <p class="lead">ASP.NET is a free web framework for building great Web sites and Web applications using HTML, CSS, and JavaScript.</p>
        <p><a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Getting started</h2>
            <p>
                ASP.NET Web Forms lets you build dynamic websites using a familiar drag-and-drop, event-driven model.
            A design surface and hundreds of controls and components let you rapidly build sophisticated, powerful UI-driven sites with data access.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div> --%>

</asp:Content>
