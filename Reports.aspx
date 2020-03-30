<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="Reports" ValidateRequest="false" %>

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
            <center><h1>Reports</h1></center>
            <%--<div class="card shadow-lg">
                
                <div class="card-body">
                    <label runat="server" class="text-danger" id="message"></label>
                    <!-- Main Content -->
                    <div class="row">
                        <div class="col-lg-2">
                            <div class="form-group">
                                <label class="control-label">ID</label>
                                <asp:TextBox ID="supplierID" runat="server" class="form-control" MaxLength="50" Disabled="true" />
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-group">
                                <label class="control-label">Company Name</label>
                                <asp:TextBox ID="txtCompanyName" runat="server" class="form-control" MaxLength="50" Placeholder="Company Name" required />
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label">TIN #</label>
                                <asp:TextBox ID="txtTin" runat="server" class="form-control" MaxLength="15" Placeholder="000-000-000-000" required />
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label">Address</label>
                                <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="5" runat="server" class="form-control" MaxLength="50" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="col-lg-4">
                        <div class="input-group">
                            <asp:Button ID="btnAdd" runat="server" class="btn btn-lg btn-success" Text="Add Supplier" OnClick="AddCompany" />
                            <asp:Button ID="btnEdit" runat="server" class="btn btn-lg btn-success" Text="Update Supplier" OnClick="SaveCompany" />
                            <asp:Button ID="btnCancel" runat="server" hidden class="btn btn-lg btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>--%>

            <!-- Table row -->
            <div class="card mt-5 bg-info">
                <div class="card-body">
                    <div class="container">
                        <div class="text-white">
                            <center><h1>Sales</h1></center>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 table">
                                <table id="dtSales" class="table table-striped">
                                    <thead>
                                        <tr style="text-align: center">
                                            <th>Company</th>
                                            <th>Tin #</th>
                                            <th>Address</th>
                                            <th>Date</th>
                                            <th>Amount</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%--OnPagePropertiesChanging="lvRates_PagePropertiesChanging"--%>
                                        <asp:ListView ID="lvSales" runat="server">
                                            <ItemTemplate>
                                                <tr class="bg-default" style="text-align: center">
                                                    <td><%# Eval("CompanyName") %></td>
                                                    <td><%# Eval("TinNo") %></td>
                                                    <td><%# Eval("Address") %></td>
                                                    <td><%# Eval("Date", "{0:MMM dd, yyyy}") %></td>
                                                    <td><%# Eval("Amount", "{0: #,##0.00}") %></td>

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

            <!-- Table row -->
            <div class="card mt-5 bg-info">
                <div class="card-body">
                    <div class="container">
                        <div class="text-white">
                            <center><h1>Purchases</h1></center>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 table">
                                <table id="dtPurchases" class="table table-striped">
                                    <thead>
                                        <tr style="text-align: center">
                                            <th>Company</th>
                                            <th>Tin #</th>
                                            <th>Address</th>
                                            <th>Date</th>
                                            <th>Amount</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <%--OnPagePropertiesChanging="lvRates_PagePropertiesChanging"--%>
                                        <asp:ListView ID="lvPurchases" runat="server">
                                            <ItemTemplate>
                                                <tr class="bg-default" style="text-align: center">
                                                    <td><%# Eval("CompanyName") %></td>
                                                    <td><%# Eval("TinNo") %></td>
                                                    <td><%# Eval("Address") %></td>
                                                    <td><%# Eval("Date", "{0:MMM dd, yyyy}") %></td>
                                                    <td><%# Eval("Amount") %></td>

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
            $('#dtSales').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    {
                        //extend: 'pdfHtml5',
                        //orientation: 'landscape',
                        //pageSize: 'Letter',
                        //title: 'WYSCO Trading - Sales Report'
                        extend: 'print',
                        orientation: 'landscape'

                    }
                ]
            });
        });
    </script>
    <%--<script>
        $(document).ready(function () {
            $('#example').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    {
                        extend: 'pdfHtml5',
                        orientation: 'landscape',
                        pageSize: 'LEGAL'
                    }
                ]
            });
        });
    </script>--%>

    <script>
        $(document).ready(function () {
            $('#dtPurchases').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    'pdf', 'print'
                ]
            });
        });
    </script>
</asp:Content>
