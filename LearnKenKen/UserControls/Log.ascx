<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Log.ascx.cs" Inherits="LearnKenKen.UserControls.Log" ViewStateMode="Disabled" %>
<div id="logContainer" runat="server" class="kenLog">
    <div class="kenLogEasiest">
        Easiest cell to solve : <span style="color:#303030;" id="spanCellId" runat="server">?</span>
    </div>
    <div class="kenLogWhy">
        <span style="font-size:1.2em;">Why ? </span>
    </div>
    <div class="kenLogWhy">        
        <span id="spanWhy" runat="server">Because... </span>
    </div>
    <div class="kenLogResult">
        <span style="font-size:1.2em;">And therefore ...</span><br />
    </div>
    <div class="kenLogResult">
        <asp:ListView runat="server" ID="lstDetails">
            <ItemTemplate>
                <div style="padding:4px;border-bottom:1px solid #C1CDCD;text-align:left;"><%# Eval("Detail") %><br /></div>
            </ItemTemplate>
        </asp:ListView>
    </div>
</div>