<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Customer" ValidateRequest="false" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <div class="header bg-gradient-gray-dark pb-5 pt-5 pt-md-8">
        <div class="container-fluid">
            <div class="header-body">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div class="container mt--7">
        <!-- page content -->
        <form runat="server" classa="form-horizontal">
            <asp:ScriptManager runat="server" />
            <%--<asp:UpdatePanel runat="server" ID="Upd1">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnAdd" />
                    <%--<asp:AsyncPostBackTrigger ControlID="btnAdd" />
                </Triggers>
                <ContentTemplate>--%>
            <div class="card shadow-lg bg-info">
                <div class="card-body">
                    <label runat="server" class="text-danger" id="message"></label>
                    <!-- Main Content -->
                    <div class="row">
                        <div class="col-lg-2">
                            <div class="form-group">
                                <label class="control-label text-darker">ID</label>
                                <asp:TextBox ID="customerID" runat="server" class="form-control" MaxLength="50" Disabled="true" />
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label class="control-label text-darker">Company Name</label>
                                <asp:TextBox ID="txtCompanyName" runat="server" class="form-control" MaxLength="50" Placeholder="Company Name" required />
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label text-darker">TIN #</label>
                                <asp:TextBox ID="txtTin" runat="server" class="form-control" MaxLength="15" Placeholder="000-000-000-000" required />
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label text-darker">Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" class="form-control" MaxLength="200" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="col-lg-4">
                        <div class="input-group">
                            <asp:Button ID="btnAdd" runat="server" class="btn btn-lg btn-warning" Text="Add Customer" OnClick="AddCompany" />
                            <asp:Button ID="btnEdit" runat="server" class="btn btn-lg btn-warning" Text="Update Customer" OnClick="SaveCompany" />
                            <asp:Button ID="btnCancel" runat="server" hidden class="btn btn-lg btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Table row -->
            <div class="card mt-5 bg-info">
                <div class="card-body">
                    <div class="container">
                        <div class="text-white">
                            <center><h1>List of Customer</h1></center>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 table">
                                <table id="dtCustomer" class="table table-striped">
                                    <thead>
                                        <tr style="text-align:center">
                                            <th>#</th>
                                            <th>Company</th>
                                            <th>Tin #</th>
                                            <th>Address</th>
                                            <th>Status</th>
                                            <th>Actions</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%--OnPagePropertiesChanging="lvRates_PagePropertiesChanging"--%>
                                        <asp:ListView ID="lvCustomer" runat="server">
                                            <ItemTemplate>
                                                <tr class="bg-default" style="text-align:center">
                                                    <td><%# Eval("CustomerID") %></td>
                                                    <td><%# Eval("CompanyName") %></td>
                                                    <td><%# Eval("TinNo") %></td>
                                                    <td><%# Eval("Address") %></td>
                                                    <td><%# Eval("Status") %></td>
                                                    <td>
                                                        <a href='Customer.aspx?EditID=<%# Eval("CustomerID") %>' class="btn btn-info btn-sm">Edit</a>&nbsp;
                                                        <a href='Customer.aspx?DeleteID=<%# Eval("CustomerID") %>' class="btn btn-danger btn-sm" onclick="return confirm('Do you want to archive this item?')">Archive</a>&nbsp;
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <EmptyDataTemplate>
                                                <tr>
                                                    <td colspan="10">
                                                        <h2 class="text-center">No records found.</h2>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                        </asp:ListView>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </div>
        </form>
    </div>
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="Server">
    <%-- for data tables --%>
    <script>
        $(document).ready(function () {
            $('#dtCustomer').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    
                ]
            });
        });
    </script>
</asp:Content>