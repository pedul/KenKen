<%@ Page Title="KenKen Without Backtracking" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="LearnKenKen._Default" %>

<%@ Register Src="~/UserControls/KenGrid.ascx" TagName="KenGrid" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/Log.ascx" TagName="Log" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/KenList.ascx" TagName="KenList" TagPrefix="uc4" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>        
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div style="float:left;" id="gridContriner">
                    <uc2:KenGrid ID="kenGrid" runat="server" />
                </div>                
                <div style="float:left;padding-left:1.5em;width:250px;text-align:center;">
                    <uc4:KenList runat="server" id="lstKen" OnKenKenSelected="lstKen_KenKenSelected" />
                    <div style="margin-top:1em;">&nbsp;</div>
                    <asp:LinkButton runat="server" ID="btnSolve" Text="Take a Step !"
                        onclick="btnSolveStepByStep_Click" class="button" Font-Underline="false" />
                    <asp:LinkButton runat="server" ID="btnJump" Text="Jump !"
                        onclick="btnJump_Click" Font-Size="1.2em" Font-Underline="false" ForeColor="#556B2F" Visible="false" />
                    <span runat="server" id="spanSolved" visible="false" style="font-size:1.5em;color:#556B2F;"></span>
                    <uc3:Log runat="server" ID="currentLog" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>