<%@ Page Title="About KenKen" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Help.aspx.cs" Inherits="LearnKenKen.Help" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <p> 
        KenKen is a mathematical puzzle from Japan which loosely means cleverness squared (ken X ken) !                                        
    </p>
    <p>
        It is like Sudoku with additional math rules, where the numbers in each cage must satisfy a mathematical equation. I first learnt about KenKen through the <a href="http://www.nytimes.com/2009/02/09/arts/09ken.html" target="_blank">New York Times</a> and have since been addicted. To know more, check out <a href="http://en.wikipedia.org/wiki/KenKen" target="_blank">Wikipedia</a>.
    </p>
    
    This program is not exactly a computerized solution to KenKen. It mimics the way a human being (mostly me) would typically solve this puzzle. This also means that the success of this approach is limited by my ability to solve the puzzle.
    For instance, I have not yet been able to solve the 7 x 7 Hard KenKen ! Any <a href="http://preetiedul.wordpress.com/2011/05/17/kenken/" target="_blank">help</a> would be appreciated ! :)
        
    <img alt="Failed to display help key" src="Images/kenkenhelp.png" />

    <h2>KenKen Rules</h2>
    <ol>
        <li>Do not repeat a number in any row or column.</li>
        <li>A 4x4 grid will use the digits 1-4, a 6x6 grid will use the digits 1-6 and so forth.</li>
        <li>The numbers in each heavily outlined set of squares, called cages, must combine (in any order) to produce the target number in the top corner of the cage using the mathematical operation indicated.</li>
        <li>Cages with just one box should be filled in with the target number in the top corner.</li>
        <li>A number can be repeated within a cage as long as it is not in the same row or column.</li>
    </ol> 
    
    <p>KenKen® is a registered trademark of Nextoy, LLC. Puzzle content ©2011 KenKen Puzzle LLC.</p>       
</asp:Content>
