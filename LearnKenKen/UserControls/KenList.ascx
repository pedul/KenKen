<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KenList.ascx.cs" Inherits="LearnKenKen.UserControls.KenList" ViewStateMode="Enabled" %>

<div runat="server" id="container" style="padding-bottom:10px;" class="kenList">
    <asp:ListView ID="lstKenKens" runat="server" 
            onselectedindexchanged="lstKenKens_SelectedIndexChanged" 
            OnSelectedIndexChanging="lstKenKens_SelectedIndexChanging">
        <ItemTemplate>
            <asp:LinkButton runat="server" CommandName="Select" Text='<%# Eval("Name") %>'/><br />            
        </ItemTemplate>
    </asp:ListView>
</div>
<div>