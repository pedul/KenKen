<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KenCell.ascx.cs" Inherits="LearnKenKen.UserControls.KenCell" ViewStateMode="Disabled" %>
<asp:Panel runat="server" EnableViewState="True">
    <div runat="server" id="cellContainer" style="width:100px;height:100px;position:relative;">             
        <div runat="server">
            <div runat="server" style="float:left;">
                <span ID="spanTargetValue" runat="server" style="font-weight:bold;font-size:1.5em;color:#3A4F63;padding-left:2px;" />
                <span ID="spanOperation" runat="server" style="font-weight:bold;font-size:1.5em;color:#3A4F63;" />
            </div>
            <div ID="spanCellId" runat="server" style="float:right;padding:2px;color:#999999;font-size:0.85em">
            </div>
        </div>
        <div runat="server" style="clear:both;">            
            <div id="spanValue" runat="server" style="color:#EEAD0E;text-align:center;font-size:3em;font-weight:bold;"/>
            <div id="spanPossibilities" runat="server" style="position:absolute;bottom:0;left:0;padding:2px;color:Green;" />
            <div id="spanUnusableNumbers" runat="server" style="position:absolute;bottom:0;right:0;padding:2px;color:#EE0000;" />
        </div>
    </div>
</asp:Panel>