<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASPLASBrowse._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="jumbotron">
    <h1>Autcrock</h1>
    <p></p>
    <p class="lead">Thanks for visiting Autcrock, the central web referral point for Mike Thomas.</p>
</div>


<div class="row">
    <div class="col-md-4">
        <h2>About me - LinkedIn</h2>
        <p></p>
        <p><a class="btn btn-primary btn-lg" href="http://www.linkedin.com/in/mjthomas1">Learn more &raquo;</a></p>
    </div>
    <div class="col-md-4">
        <h2>My Soundcloud page</h2>
        <p></p>
        <p><a class="btn btn-primary btn-lg" href="http://soundcloud.com/autcrock">Learn more &raquo;</a></p>
    </div>
    <div class="col-md-4">
        <h2>Buy my music</h2>
        <p></p>
        <p><a class="btn btn-primary btn-lg" href="http://michaeljosephthomas.bandcamp.com/">Learn more &raquo;</a></p>
    </div>
</div>

<div class="row"> </div>
<div class="row">
    <h1>LAS file reader workflow (left to right)</h1>

    <div class="col-md-4">
        <asp:FileUpload class="btn btn-primary btn-lg" runat="server" ID="LASSelector"> </asp:FileUpload>
    </div>
    <div class="col-md-4">
        <asp:Button class="btn btn-primary btn-lg" runat ="server" id="UploadButton" text="Upload the file" onclick="UploadButton_Click" />
        <p></p>
        <asp:Label class="btn btn-primary btn-lg" runat="server" id="StatusLabel" text="LAS file upload status: " />
    </div>
    <div class="col-md-4">
        <asp:PlaceHolder runat="server" ID ="DisplayCodeLocation"/>
    </div>
</div>

<div class="row"> </div>
<div class="row">
</div>

 
</asp:Content>
