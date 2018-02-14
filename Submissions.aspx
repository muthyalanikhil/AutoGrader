<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Submissions.aspx.cs" Inherits="Submissions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <div>
        <br />
        <br />
        <div style="text-align: center">
            <h3>Assignment Details</h3>
        </div>
        <br />
        <asp:GridView ID="studentGridView" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:BoundField DataField="assignmentId" HeaderText="Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                <asp:BoundField DataField="assignmentName" HeaderText="Name" />
                <asp:BoundField DataField="assignmentDescription" HeaderText="Description" />
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="DownloadFile"
                            CommandArgument='<%# Eval("assignmentId") %>'></asp:LinkButton>
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
        <hr />
        <div class="container">
            <asp:PlaceHolder ID="resultPH" runat="server" Visible="false">
                <div class="panel panel-default">
                    <div class="panel-heading" style="align-content: center; font-weight: bold">Graded Percentage</div>
                    <div class="panel-body">
                        <asp:Label ID="percentLBL" runat="server" Text="Not Yet Graded...!!!"></asp:Label><br />
                        <asp:LinkButton ID="downloadSubmission" runat="server" OnClick="DownloadZip">Click to download submitted assignment</asp:LinkButton>
                    </div>
                </div>
                <hr />
            </asp:PlaceHolder>
        </div>
        <h3>&nbsp;Upload ZIP file here for submitting assignment</h3>
        <asp:FileUpload ID="uploadZIPButton" runat="server" Width="316px" CssClass="btn btn-success" />
        <br />
        <br />
        <asp:Button ID="uploadAssignment" CssClass="btn btn-success" runat="server" Text="Submit Assignment" OnClick="submitAssignment_Click" />
    </div>
</asp:Content>

