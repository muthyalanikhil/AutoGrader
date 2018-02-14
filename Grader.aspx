<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Grader.aspx.cs" Inherits="Grader" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hiddencol {
            display: none;
        }
    </style>
    <h3 style="text-align: center;">Assignment Submissions</h3>
    <br />
    <div class="clearfix">
        <button type="button" class="btn btn-success float-right" data-toggle="modal" data-target=".myModal"><i class="fa fa-plus"></i>Upload test cases</button>
    </div>

    <br />
    <h3 style="color: peru">
        <asp:Label ID="noSubmissionsLBL" runat="server" Text=""></asp:Label></h3>
    <asp:PlaceHolder runat="server" ID="partialGradePH"  Visible="false">
        <div class="container">
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">
                        <div class="panel panel-default">
                            <div class="panel-heading" style="align-content: center; font-weight: bold">Grade the student</div>
                            <div class="panel-body">
                                <asp:Label ID="nameLBL" runat="server" Text="Name:"></asp:Label><br />
                                <asp:Label ID="assignmentNameLBL" runat="server" Text="Assignment:"></asp:Label><br />
                                <asp:Label ID="Label1" runat="server" Text="Marks: "></asp:Label>
                                <asp:TextBox ID="marksTB" runat="server"></asp:TextBox>  
                                <br /><br />
                                 <asp:Button ID="updateGrade" runat="server" Text="Update" OnClick="updateGrade_Click" CssClass="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
    </asp:PlaceHolder>
    <asp:GridView ID="studentGridView" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None" Style="color: #333333; width: 98%; margin-left: 10px;">
        <AlternatingRowStyle BackColor="#f2f4f7" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="assignmentId" HeaderText="Assignment Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
            <asp:BoundField DataField="id" HeaderText="User Id" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
            <asp:BoundField DataField="userName" HeaderText="Student Name" />
            <asp:BoundField DataField="assignmentName" HeaderText="Assignment Name" />
            <asp:BoundField DataField="points" HeaderText="Points" />
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="lnkDownload" runat="server" Text="Grade" OnClick="GradeAssignment" CssClass="btn btn-primary"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="partialGrade" runat="server" Text="Add Partial Grade" OnClick="partialGrade_Click" CssClass="btn btn-primary"></asp:Button>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#6399c5" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#6399c5" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#eeeeee" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <br />
    <div>
        <hr />
        <asp:PlaceHolder ID="editGrade" runat="server">
            <div>
            </div>
        </asp:PlaceHolder>
        <div>
            <asp:PlaceHolder ID="testcasePHMain" runat="server">
                <div class="row">
                    <div class="col-md-4">
                        <asp:PlaceHolder ID="testCasePH1" runat="server" Visible="false">
                            <div class="container" style="width: 100%;">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="align-content: center; font-weight: bold">Test Case 1</div>
                                    <div class="panel-body">
                                        <asp:Label ID="output1" runat="server" Text="No test case provided"></asp:Label><br />
                                        <asp:LinkButton ID="si1Link" runat="server" title="Sample Input" data-toggle="popover" data-trigger="hover">Sample Input</asp:LinkButton><br />
                                        <asp:LinkButton ID="so1Link" runat="server" title="Expected Output" data-toggle="popover" data-trigger="hover">Expected output</asp:LinkButton><br />
                                        <asp:LinkButton ID="actualOutput1" runat="server" title="Actual Output" data-toggle="popover" data-trigger="hover">Actual output</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                    <div class="col-md-4">
                        <asp:PlaceHolder ID="testCasePH2" runat="server" Visible="false">
                            <div class="container" style="width: 100%;">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="align-content: center; font-weight: bold">Test Case 2</div>
                                    <div class="panel-body">
                                        <asp:Label ID="output2" runat="server" Text="No test case provided"></asp:Label><br />
                                        <asp:LinkButton ID="si2Link" runat="server" title="Sample Input" data-toggle="popover" data-trigger="hover">Sample Input</asp:LinkButton><br />
                                        <asp:LinkButton ID="so2Link" runat="server" title="Expected Output" data-toggle="popover" data-trigger="hover">Expected output</asp:LinkButton><br />
                                        <asp:LinkButton ID="actualOutput2" runat="server" title="Actual Output" data-toggle="popover" data-trigger="hover">Actual output</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                    <div class="col-md-4">
                        <asp:PlaceHolder ID="testCasePH3" runat="server" Visible="false">
                            <div class="container" style="width: 100%;">
                                <div class="panel panel-default">
                                    <div class="panel-heading" style="align-content: center; font-weight: bold">Test Case 3</div>
                                    <div class="panel-body">
                                        <asp:Label ID="output3" runat="server" Text="No test case provided"></asp:Label><br />
                                        <asp:LinkButton ID="si3Link" runat="server" title="Sample Input" data-toggle="popover" data-trigger="hover">Sample Input</asp:LinkButton><br />
                                        <asp:LinkButton ID="so3Link" runat="server" title="Expected Output" data-toggle="popover" data-trigger="hover">Expected output</asp:LinkButton><br />
                                        <asp:LinkButton ID="actualOutput3" runat="server" title="Actual Output" data-toggle="popover" data-trigger="hover">Actual output</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
                <hr />
                <div class="container">
                    <asp:PlaceHolder ID="outputPH" runat="server" Visible="false">
                        <div class="panel panel-default">
                            <div class="panel-heading" style="align-content: center; font-weight: bold">Total average percentage</div>
                            <div class="panel-body">
                                <asp:Label ID="percentLBL" runat="server" Text="No test case provided"></asp:Label>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </asp:PlaceHolder>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-4">
        </div>
        <div class="col-lg-4">
            <div class="col-lg-4 modal1 fade myModal1" style="width: 100%; margin-top: 10%;">
                <div class="modal-content">
                    <!-- Modal body -->
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="date">Sample Input:</label>
                                    <asp:FileUpload ID="sampleInputFU1" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <label for="date">Sample Output:</label>
                                    <asp:FileUpload ID="sampleOutputFU1" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="date">Sample Input:</label>
                                    <asp:FileUpload ID="sampleInputFU2" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <label for="date">Sample Output:</label>
                                    <asp:FileUpload ID="sampleOutputFU2" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label for="date">Sample Input:</label>
                                    <asp:FileUpload ID="sampleInputFU3" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <label for="date">Sample Output:</label>
                                    <asp:FileUpload ID="sampleOutputFU3" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Modal footer -->
                    <div class="modal-footer">
                        <asp:Button ID="updateTestCases" class="btn btn-success" runat="server" Text="Add" OnClick="updateTestCases_Click" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
        </div>
    </div>
    <script>
        $(document).ready(function () {
            $('[data-toggle="popover"]').popover();
        });
    </script>
</asp:Content>
