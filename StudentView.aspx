<%@ Page Title="Student" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="StudentView.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <br />
    <div style="text-align: center;">
        <h3>List of Assignments</h3>
    </div>
    <br />
    <asp:GridView ID="assignmentsGridView" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="assignmentId" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
            <asp:HyperLinkField DataNavigateUrlFields="assignmentId" DataNavigateUrlFormatString="Submissions.aspx?assignmentId={0}" DataTextField="assignmentName" HeaderText="Assignment Name" />
            <asp:BoundField DataField="assignmentDescription" HeaderText="Description" />
            <asp:BoundField DataField="comments" HeaderText="Status" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                        CommandArgument='<%# Eval("assignmentId") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center"></ItemStyle>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#39de9d" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#39de9d" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#eeeeee" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
</asp:Content>
