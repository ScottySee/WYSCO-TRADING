<%@ Page Title="" Language="C#" MasterPageFile="~/Home.Master" AutoEventWireup="true" CodeFile="Sales.aspx.cs" Inherits="Sales" ValidateRequest="false" %>

<asp:Content ContentPlaceHolderID="header" runat="server">
    <div class="header bg-gradient-gray-dark pb-5 pt-5 pt-md-8">
        <div class="container-fluid">
            <div class="header-body">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <div class="container mt--7" >
        <form runat="server" classa="form-horizontal">
            <asp:ScriptManager runat="server" />
            <div class="card shadow-lg bg-info">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-1">
                            <div class="form-group">
                                <label class="control-label text-darker">ID</label>
                                <asp:TextBox ID="salesID" runat="server" class="form-control" MaxLength="50" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label text-darker">Company Name</label>
                                <div class="text-black-50">
<asp:TextBox ID="txtcompanyname" runat="server" class="form-control" MaxLength="50" Disabled="true" />
                                </div>
                                
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="form-group">
                                <label class="control-label text-darker">TIN #</label>
                                <asp:TextBox ID="txtTin" runat="server" class="form-control" MaxLength="50" Disabled="true" />
                            </div>
                        </div>
                        <div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label text-darker">Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" class="form-control" MaxLength="200" Disabled="true" />
                            </div>
                        </div>
                        <%--<div class="col-lg-4">
                            <div class="form-group">
                                <label class="control-label">Month</label>
                                <asp:DropDownList ID="ddlMonth" runat="server" class="form-control" required>
                                    <asp:ListItem Value="" style="color: black">---------------Select Month---------------</asp:ListItem>
                                    <asp:ListItem Value="January" style="color: black">January</asp:ListItem>
                                    <asp:ListItem Value="February" style="color: black">February</asp:ListItem>
                                    <asp:ListItem Value="March" style="color: black">March</asp:ListItem>
                                    <asp:ListItem Value="April" style="color: black">April</asp:ListItem>
                                    <asp:ListItem Value="May" style="color: black">May</asp:ListItem>
                                    <asp:ListItem Value="June" style="color: black">June</asp:ListItem>
                                    <asp:ListItem Value="July" style="color: black">July</asp:ListItem>
                                    <asp:ListItem Value="August" style="color: black">August</asp:ListItem>
                                    <asp:ListItem Value="September" style="color: black">September</asp:ListItem>
                                    <asp:ListItem Value="October" style="color: black">October</asp:ListItem>
                                    <asp:ListItem Value="November" style="color: black">November</asp:ListItem>
                                    <asp:ListItem Value="December" style="color: black">December</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="col-lg-3">
                            <div class="form-group">
                                <label class="control-label">Date:</label>
                                <asp:TextBox TextMode="date" ID="date" runat="server" class="form-control" min="01-01-2020" max="12-31-2040" required />
                            </div>
                        </div>
                        <div class="col-lg-2">
                            <div class="form-group">
                                <label class="control-label text-darker">Amount</label>
                                <asp:TextBox ID="txtAmount" runat="server" class="form-control" type="number" min="0.01" max="1000000.00" step="0.01" required />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="col-lg-4">
                        <div class="input-group">
                            <%--<asp:Button ID="btnAdd" runat="server" class="btn btn-lg btn-success" Text="Add Product" OnClick="AddProduct" />--%>
                            <asp:Button ID="btnEdit" runat="server" OnClientClick="return confirm('Save changes?')" class="btn btn-lg btn-warning" Text="Update Sales" OnClick="SaveSales" />
                            <asp:Button ID="btnCancel" runat="server" hidden class="btn btn-lg btn-danger" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Table row -->
            <div class="card mt-5 bg-info">
                <div class="card-body">
                    <div class="container">
                        <%--<div class="col-lg-offset-6 col-lg-3 text-white">
                            START<asp:TextBox ID="txtStart" runat="server" CssClass="form-control"
                                type="date" AutoPostBack="true" OnTextChanged="SearchByDate" />
                        </div>
                        <br />
                        <div class="col-lg-3 text-white">
                            END<asp:TextBox ID="txtEnd" runat="server" CssClass="form-control"
                                type="date" AutoPostBack="true" OnTextChanged="SearchByDate" />
                        </div>--%>
                        <span class="pull-right">
                            <a href="PrintSales.aspx?ID=<%= Request.QueryString["ID"] %>" class="btn btn-sm btn-success btn-block">
                                Print Sales
                            </a>
                        </span>
                        <div class="text-white">
                            <center><h1>Sales</h1></center>
                            
                        </div>
                        <div class="row">
                            <div class="col-xs-12 table">
                                <table id="dtSales" class="table table-striped">
                                    <thead style="text-align:center">
                                        <%--<th>#</th>--%>
                                        <th>Company Name</th>
                                        <th>TIN #</th>
                                        <th>Address</th>
                                        <th>Amount</th>
                                        <th>Date</th>
                                        <th>Actions</th>
                                    </thead>
                                    <tbody>
                                        <asp:ListView ID="lvSales" runat="server">
                                            <ItemTemplate>
                                                <tr style="text-align:center">
                                                    <%--<td><%# Eval("SalesID") %></td>--%>
                                                    <td><%# Eval("CompanyName") %></td>
                                                    <td><%# Eval("TinNo") %></td>
                                                    <td><%# Eval("Address") %></td>
                                                    <td>Php <%# Eval("Amount", "{0: #,##0.00}") %></td>
                                                    <td><%# Eval("Date", "{0:MMM dd, yyyy}") %></td>
                                                    <td>
                                                        <a href='Sales.aspx?EditID=<%# Eval("SalesID") %>' class="btn btn-info btn-sm"><i class="fa fa-edit"></i>Edit</a>&nbsp;
                                                <%--<a href='products.aspx?DeleteID=<%# Eval("ProductID") %>' class="btn btn-danger btn-sm" onclick="return confirm('Do you want to delete this item?')"><i class="fa fa-trash"></i>Delete</a>&nbsp;--%>
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
                </div>
            </div>
            <triggers>
                <asp:PostBackTrigger ControlID="btnAdd" />
            </triggers>
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

                ]
            });
        });
    </script>
</asp:Content>