<%@ Page Title="Admin" EnableEventValidation="true"  Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AdminView.aspx.cs" Inherits="AdminView" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <div style="text-align: center;">
        <h2 style="align-content: center">Lab Assignments</h2>
    </div>
    <h3 style="padding-left: 10px;">Add an assignment</h3>
    <div style="padding-left: 20px;">
        <h5>Assignment Name:
            <asp:TextBox ID="assignmentName" runat="server" Height="22px" Width="200px"></asp:TextBox></h5>
        <h5>Description    :
            <asp:TextBox ID="assignmentDescription" runat="server" Height="22px" Width="400px"></asp:TextBox></h5>
        <p>&nbsp;</p>
        <asp:FileUpload ID="fileUploadButton" runat="server" CssClass="btn btn-success" /><br />
        <asp:Label ID="maxfilesize" runat="server" Visible="false" CssClass="alert alert-warning"></asp:Label>
        <br /><br />
        <asp:Button ID="createAssignment" runat="server" Text="Create Assignment" CssClass="btn btn-success" OnClick="createAssignment_Click" />
    </div>
    <hr />
    <div>
        <asp:GridView ID="assignmentsGridView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="assignmentId" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol"></asp:BoundField>
                <asp:HyperLinkField DataNavigateUrlFields="assignmentId" DataNavigateUrlFormatString="Grader.aspx?assignmentId={0}" DataTextField="assignmentName" HeaderText="Assignment Name" />
                <asp:BoundField DataField="assignmentDescription" HeaderText="Description" />
                <asp:BoundField DataField="createdBy" HeaderText="Created by"></asp:BoundField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                            CommandArgument='<%# Eval("assignmentId") %>'></asp:LinkButton>
                    </ItemTemplate>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="deleteAssignment" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="deleteAssignment_Click"></asp:Button>
                </ItemTemplate>
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
    </div>
</asp:Content>
