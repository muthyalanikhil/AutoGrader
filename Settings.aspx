<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeFile="Settings.aspx.cs" Inherits="Settings" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3 style="text-align: center;">Add Instructors</h3>
    <br />
    <h4 style="text-align: center;">Click on arrow to switch user roles</h4>
    <div class="row">
        <div class="col-md-6 col-sm-12">
            <asp:GridView ID="allUsersListGV" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="userName" HeaderText="List of users"></asp:BoundField>
                    <asp:TemplateField HeaderStyle-Width="40">
                        <ItemTemplate>
                            <asp:ImageButton ID="moveToAdmin" runat="server"
                                ImageUrl="~/images/moveRight.png" OnClick="moveToAdmin_Click" ToolTip="mark as instructor"
                                CommandArgument='<%#Bind("id")%>' />
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
        <div class="col-md-6 col-sm-12">
            <asp:GridView ID="adminListGV" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40">
                        <ItemTemplate>
                            <asp:ImageButton ID="moveToStudent" runat="server"
                                ImageUrl="~/images/moveLeft.png" OnClick="moveToStudent_Click" ToolTip="mark as student"
                                CommandArgument='<%#Bind("id")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="userName" HeaderText="List of Admins"></asp:BoundField>
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
    </div>
</asp:Content>
