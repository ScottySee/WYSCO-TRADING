<%@ Page Title="" Language="C#" MasterPageFile="~/PrintPage.Master" AutoEventWireup="true" CodeFile="PrintPurchases.aspx.cs" Inherits="PrintPurchases" %>

<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="Server">
    <div class="header bg-gradient-gray-dark pb-5 pt-5 pt-md-8">
        <div class="container-fluid">
            <div class="header-body">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="Server">
    <div class="container mt--7">
        <!-- page content -->
        <form runat="server" class="form-horizontal">
            <asp:ScriptManager runat="server" />
            <asp:UpdatePanel ID="Sales" runat="server">
                <ContentTemplate>
                    <div class="card shadow-lg">
                        <div class="card-body">
                            <div class="container" id="printpage">
                                <div class="row m-3">
                                    <div class="container">
                                        <div class="text-center">
                                            <h1><strong>WYSCO TRADING</strong></h1>
                                            <h1>Purchases</h1>
                                            <p>
                                                <h2><em><%= Date %></em></h2>
                                            </p>
                                        </div>
                                        <table class="table-active table" style="width: 100%;">
                                            <thead>
                                                <tr class="text-center">
                                                    <th>Company</th>
                                                    <th>TIN #</th>
                                                    <th>Address</th>
                                                    <th>Amount</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:ListView ID="lvPrintOrders" runat="server">
                                                    <ItemTemplate>
                                                        <tr class="text-center">
                                                            <td class="text-left"><%# Eval("CompanyName") %></td>
                                                            <td class="text-left"><%# Eval("TinNo") %></td>
                                                            <td class="text-left"><%# Eval("Address") %></td>
                                                            <td class="text-right"><%# Eval("Amount", "{0: #,##0.00}") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr>
                                                            <td colspan="6">
                                                                <h3 class="text-center">No records found.
                                                                </h3>
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </tbody>
                                            <tfoot>
                                                <tr class="text-center">
                                                    <th></th>
                                                    <th></th>
                                                    <th class="text-right">Total</th>
                                                    <td class="text-right">Php <%=TotalAmount.ToString("#,##0.00") %></td>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <input name="b_print" type="button" class="ipt btn btn-success" onclick="printdiv('printpage');" value="Print" />
                            <a href="Purchases.aspx" class="ipt btn btn-success">Back</a>
                        </br>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="Server">
    <script>
        function printdiv(printpage) {
            var headstr = "<html><head><title>Order Receipt</title></head><body>";
            var footstr = "</body>"
            var newstr = document.getElementById(printpage).innerHTML;
            var oldstr = document.body.innerHTML
            document.body.innerHTML = headstr + newstr + footstr;
            window.print();
            document.body.innerHTML = oldstr;
        }
    </script>
    <script>
        $('#lvPrintOrders').DataTable( {
            columnDefs: [
                {
                    targets: -1,
                    className: 'dt-body-right'
                }
            ]
} );
    </script>
</asp:Content>
