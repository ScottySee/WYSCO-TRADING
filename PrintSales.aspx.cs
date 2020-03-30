using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PrintSales : System.Web.UI.Page
{
    public string Date;
    public string Company;
    public string TinNo;
    public string Address;
    public double TotalAmount;

    SqlCommand cmd;
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Request.QueryString["ID"] != null)
        //{
        //    int ID = 0;
        //    bool check = int.TryParse(Request.QueryString["ID"], out ID);
        //    if (check != false)
        //    {
        //        GetOrderDetails(ID);
        //    }

        //}
        //else
        //{

        //    Response.Redirect("Sales.aspx");
        //}
        GetOrderDetails();
    }

    //void GetSales()
    //{
    //    using (SqlConnection con = new SqlConnection(Util.GetConnection()))
    //    //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
    //    {
    //        con.Open();
    //        string query = @"SELECT * FROM Sales WHERE Status != 'Archived'";

    //        using (SqlCommand cmd = new SqlCommand(query, con))
    //        {
    //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
    //            {
    //                DataSet ds = new DataSet();
    //                da.Fill(ds, "Sales");
    //                lvPrintOrders.DataSource = ds;
    //                lvPrintOrders.DataBind();
    //            }
    //        }
    //    }
    //}

    void GetOrderDetails()
    {

        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            string query = @"SELECT c.CompanyName, c.TinNo, c.Address, FORMAT (Date, 'MMMM yyyy') as Date, Amount FROM Sales s
                                INNER JOIN Customer c ON s.CustomerID=c.CustomerID
                                WHERE Amount!='0.00'";

            cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@SalesNo", ID);
            con.Open();

            using (SqlDataReader data = cmd.ExecuteReader())
            {
                while (data.Read())
                {
                    Company = data["CompanyName"].ToString();
                    TinNo = data["TinNo"].ToString();
                    Address = data["Address"].ToString();
                    Date = data["Date"].ToString();
                    TotalAmount = TotalAmount + Convert.ToDouble(data["Amount"].ToString());


                }

            }
            using (SqlDataReader data = cmd.ExecuteReader())
            {
                lvPrintOrders.DataSource = data;
                lvPrintOrders.DataBind();
            }

            con.Close();

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sales.aspx");
    }
}