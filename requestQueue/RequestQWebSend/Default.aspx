<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RequestQWebSend._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to RequestQ Send a Message!
    </h2>
    <p>
        <asp:DropDownList ID="ddlPriority" runat="server">
            <asp:ListItem Selected="True" Value="1">File Process</asp:ListItem>
            <asp:ListItem Value="2">Print Batch</asp:ListItem>
            <asp:ListItem Value="3">File Validation</asp:ListItem>
        </asp:DropDownList>
&nbsp;
        <asp:Button ID="btnSend" runat="server" Text="Send" OnClick="btnSend_OnClick"/>
    </p>
    <p>
        <asp:GridView ID="Gridview1" runat="server">
        </asp:GridView>

        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_OnClick"/>
        </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
        
    </p>
</asp:Content>
