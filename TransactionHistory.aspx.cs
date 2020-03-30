using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TransactionHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetSales();
            GetPurchases();
        }
        //GetSales();
        //GetPurchases();
    }

    void GetSales()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            //string query1 = @"SELECT DISTINCT s.SalesID, c.CompanyName, c.TinNo, 
            //                    c.Address, s.Amount, s.Date FROM Sales s
            //                    INNER JOIN Customer c on s.CustomerID=c.CustomerID
            //                    ORDER BY CompanyName ASC";

            //string query = @"SELECT * FROM SalesHistory";

            string query = @"SELECT sh.SalesHistoryID, c.CompanyName, sh.Date, sh.Amount, sh.DateAdded FROM SalesHistory sh
                                INNER JOIN Sales s ON s.SalesID=sh.SalesID
                                INNER JOIN Customer c ON c.CustomerID=s.CustomerID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "SalesHistory");
                    lvSales.DataSource = ds;
                    lvSales.DataBind();
                }
            }
        }
    }

    void GetSales(DateTime start, DateTime end)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            //string query1 = @"SELECT DISTINCT s.SalesID, c.CompanyName, c.TinNo, 
            //                    c.Address, s.Amount, s.Date FROM Sales s
            //                    INNER JOIN Customer c on s.CustomerID=c.CustomerID
            //                    ORDER BY CompanyName ASC";

            //string query = @"SELECT * FROM SalesHistory";

            string query = @"SELECT sh.SalesHistoryID, c.CompanyName, sh.Date, sh.Amount, sh.DateAdded FROM SalesHistory sh
                                INNER JOIN Sales s ON s.SalesID=sh.SalesID
                                INNER JOIN Customer c ON c.CustomerID=s.CustomerID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "SalesHistory");
                    lvSales.DataSource = ds;
                    lvSales.DataBind();
                }
            }
        }
    }

    void GetPurchases()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            string query = @"SELECT ph.PurchasesHistoryID, s.CompanyName, ph.Date, ph.Amount, ph.DateAdded FROM PurchasesHistory ph
                                INNER JOIN Purchases p ON p.PurchaseID=ph.PurchaseID
                                INNER JOIN Supplier s ON s.SupplierID=p.SupplierID";

            //string query1 = @"SELECT DISTINCT p.PurchaseID, s.CompanyName, s.TinNo, 
            //                    s.Address, p.Amount, p.Date FROM Purchases p
            //                    INNER JOIN Supplier s on s.SupplierID=p.SupplierID
            //                    ORDER BY CompanyName ASC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "PurchasesHistory");
                    lvPurchases.DataSource = ds;
                    lvPurchases.DataBind();
                }
            }
        }
    }

    void GetPurchases(DateTime start1, DateTime end1)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            //string query = @"SELECT * FROM Purchases WHERE Status != 'Archived'";

            string query = @"SELECT ph.PurchasesHistoryID, s.CompanyName, ph.Date, ph.Amount, ph.DateAdded FROM PurchasesHistory ph
                                INNER JOIN Purchases p ON p.PurchaseID=ph.PurchaseID
                                INNER JOIN Supplier s ON s.SupplierID=p.SupplierID";

            //string query1 = @"SELECT DISTINCT p.PurchaseID, s.CompanyName, s.TinNo, 
            //                    s.Address, p.Amount, p.Date FROM Purchases p
            //                    INNER JOIN Supplier s on s.SupplierID=p.SupplierID
            //                    ORDER BY CompanyName ASC";

            // string query = @"SELECT * FROM PurchasesHistory";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "PurchasesHistory");
                    lvPurchases.DataSource = ds;
                    lvPurchases.DataBind();
                }
            }
        }
    }

    protected void SearchBySales(object sender, EventArgs e)
    {
        DateTime start = DateTime.Now;
        DateTime end = DateTime.Now;

        bool validStart = DateTime.TryParse(txtStart.Text, out start);
        bool validEnd = DateTime.TryParse(txtEnd.Text, out end);

        if (validStart && validEnd)
        {
            // search records by date range
            GetSales(start, end);
        }
        else
        {
            // use default
            GetSales();
        }
    }

    protected void SearchByPurchases(object sender, EventArgs e)
    {
        DateTime start1 = DateTime.Now;
        DateTime end1 = DateTime.Now;

        bool validStart = DateTime.TryParse(Start.Text, out start1);
        bool validEnd = DateTime.TryParse(End.Text, out end1);

        if (validStart && validEnd)
        {
            // search records by date range
            GetSales(start1, end1);
        }
        else
        {
            // use default
            GetSales();
        }
    }
}