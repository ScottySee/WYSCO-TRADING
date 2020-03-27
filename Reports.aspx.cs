using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class Reports : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetSales();
        GetPurchases();
    }

    void GetSales()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            string query = @"SELECT DISTINCT s.SalesID, c.CompanyName, c.TinNo, 
                                c.Address, s.Amount, s.Date FROM Sales s
                                INNER JOIN Customer c on s.CustomerID=c.CustomerID
                                ORDER BY CompanyName ASC";

            //string query = @"SELECT DISTINCT s.SalesID, c.CompanyName, c.TinNo, 
            //                    c.Address, s.Amount, s.Date FROM Sales s
            //                    INNER JOIN Customer c on s.CustomerID=c.CustomerID
            //                    ORDER BY CompanyName ASC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Sales");
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
            //string query = @"SELECT * FROM Purchases WHERE Status != 'Archived'";

            string query = @"SELECT DISTINCT p.PurchaseID, s.CompanyName, s.TinNo, 
                                s.Address, p.Amount, p.Date FROM Purchases p
                                INNER JOIN Supplier s on s.SupplierID=p.SupplierID
                                ORDER BY CompanyName ASC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Purchases");
                    lvPurchases.DataSource = ds;
                    lvPurchases.DataBind();
                }
            }
        }
    }
}