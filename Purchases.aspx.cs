﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Purchases : System.Web.UI.Page
{
    int editID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            btnEdit.Visible = false;
            //FOR VIEWING THE EDIT INTERFACE
            //EDITVIEW START
            if (Request.QueryString["EditID"] != null)
            {

                bool validPurchases = int.TryParse(Request.QueryString["EditID"].ToString(), out editID);

                if (validPurchases)
                {
                    btnEdit.Visible = true;
                    EditPurchases(editID);
                }
                else
                    Response.Redirect("Purchases.aspx");
            }
            //EDITVIEW END
            //DELETE ID CHECKING START
            //else
            //{
            //    //int DeleteID = 0;
            //    //bool validOrders = int.TryParse(Request.QueryString["DeleteID"].ToString(), out DeleteID);

            //    //if (validOrders)
            //    //{
            //    //    DeleteRecord(DeleteID);
            //    //}
            //    //else
            //        Response.Redirect("OrdersWarehouseAdmin.aspx");
            //}
            //DELETE ID CHECKING END
            GetPurchases();
        }
        //FOR VIEWING
        GetPurchases();
    }

    void GetPurchases()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            //string query1 = @"SELECT DISTINCT o.OrderNo, o.DateOrdered, o.PaymentMethod, 
            //                    u.Lastname + ', ' + u.Firstname AS CustomerName,
            //                    (SELECT SUM(Amount) FROM OrderDetails WHERE OrderNo= o.OrderNo) AS TotalAmount,
            //                    o.Status FROM Orders o
            //                    INNER JOIN OrderDetails od ON o.OrderNo= od.OrderNo
            //                    INNER JOIN Users u ON od.UserID = u.UserID
            //                    ORDER BY o.DateOrdered DESC";

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

    //void GetOrders(DateTime start, DateTime end)
    //{
    //    using (SqlConnection con = new SqlConnection(Util.GetConnection()))
    //    {
    //        con.Open();
    //        string query = @"SELECT DISTINCT o.OrderNo, o.DateOrdered, o.PaymentMethod,
    //                             u.LastName + ', ' + u.FirstName AS CustomerName,
    //                             (SELECT SUM(Amount) FROM OrderDetails WHERE OrderNo= o.OrderNo) AS TotalAmount,
    //                             o.Status FROM Orders o
    //                             INNER JOIN OrderDetails od ON o.OrderNo= od.OrderNo
    //                             INNER JOIN Users u ON od.UserID = u.UserID
    //                             WHERE o.DateOrdered BETWEEN @start AND @end
    //                             ORDER BY o.DateOrdered DESC";

    //        using (SqlCommand cmd = new SqlCommand(query, con))
    //        {
    //            cmd.Parameters.AddWithValue("@start", start);
    //            cmd.Parameters.AddWithValue("@end", end.AddHours(23).AddMinutes(59).AddSeconds(59));

    //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
    //            {
    //                DataSet ds = new DataSet();
    //                da.Fill(ds, "Orders");
    //                lvSales.DataSource = ds;
    //                lvSales.DataBind();
    //            }
    //        }
    //    }
    //}

    //protected void SearchByDate(object sender, EventArgs e)
    //{
    //    DateTime start = DateTime.Now;
    //    DateTime end = DateTime.Now;

    //    bool validStart = DateTime.TryParse(txtStart.Text, out start);
    //    bool validEnd = DateTime.TryParse(txtEnd.Text, out end);

    //    if (validStart && validEnd)
    //    {
    //        // search records by date range
    //        GetOrders(start, end);
    //    }
    //    else
    //    {
    //        // use default
    //        GetOrders();
    //    }
    //}

    protected void EditPurchases(int ID)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            //string query1 = @"SELECT * FROM Orders WHERE OrderNo=@OrderNo";

            string query = @"SELECT DISTINCT p.PurchaseID, s.CompanyName, s.TinNo, 
                                s.Address, p.Date FROM Purchases p
                                INNER JOIN Supplier s ON s.SupplierID = p.SupplierID
                                WHERE PurchaseID=@PurchaseID";

            //string query2 = @"SELECT DISTINCT s.SalesID, c.CompanyName, c.TinNo, 
            //                    c.Address, s.Amount FROM Sales s
            //                    INNER JOIN Customer c ON c.CustomerID = s.CustomerID
            //                    ORDER BY c.CompanyName ASC";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@PurchaseID", ID);
                using (SqlDataReader data = cmd.ExecuteReader())
                {
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            purchaseID.Text = data["PurchaseID"].ToString();
                            txtcompanyname.Text = data["CompanyName"].ToString();
                            txtTin.Text = data["TinNo"].ToString();
                            txtAddress.Text = data["Address"].ToString();
                        }
                    }
                    else
                    {
                        Response.Redirect("Purchases.aspx");
                    }
                }
            }
        }
    }
    protected void SavePurchase(object sender, EventArgs e)
    {
        if (txtAmount.Text == "0")
        {
            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Purchases SET Amount=@Amount, Date=@Date, DateModified=@DateModified WHERE PurchaseID=@PurchaseID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    //cmd.Parameters.AddWithValue("@Month", ddlMonth.SelectedValue);
                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@Date", date.Text);
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID.Text);
                    //cmd.Parameters.AddWithValue("@Image", "Sample.jpg");

                    cmd.ExecuteNonQuery();
                    Response.Redirect("Purchases.aspx");
                }
            }
        }
        if (txtAmount.Text != "0")
        {
            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Purchases SET Amount=@Amount, Date=@Date, DateModified=@DateModified WHERE PurchaseID=@PurchaseID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    //cmd.Parameters.AddWithValue("@Month", ddlMonth.SelectedValue);
                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@Date", date.Text);
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID.Text);
                    //cmd.Parameters.AddWithValue("@Image", "Sample.jpg");

                    cmd.ExecuteNonQuery();
                }
            }

            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO PurchasesHistory VALUES (@PurchaseID, @Date, @Amount, @DateAdded)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@PurchaseID", purchaseID.Text);
                    cmd.Parameters.AddWithValue("@Date", date.Text);
                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    Response.Redirect("Purchases.aspx");
                }
            }
        }
        //using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //{
        //    con.Open();
        //    string query = @"INSERT INTO PurcahsesHistory VALUES (@CompanyName, @TinNo, @Address, @Amount, @Date, @Status, @DateAdded)";

        //    using (SqlCommand cmd = new SqlCommand(query, con))
        //    {
        //        //cmd.Parameters.AddWithValue("@Month", ddlMonth.SelectedValue);
        //        cmd.Parameters.AddWithValue("@CompanyName", txtcompanyname.Text);
        //        cmd.Parameters.AddWithValue("@TinNo", txtTin.Text);
        //        cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
        //        cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
        //        cmd.Parameters.AddWithValue("@Date", date.Text);
        //        cmd.Parameters.AddWithValue("@Status", "Active");
        //        cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
        //        //cmd.Parameters.AddWithValue("@SalesID", salesID.Text);
        //        //cmd.Parameters.AddWithValue("@Image", "Sample.jpg");

        //        cmd.ExecuteNonQuery();

        //        Response.Redirect("Purchases.aspx");
        //    }
        //}
    }

    //void DeleteRecord(int ID)
    //{
    //    using (SqlConnection con = new SqlConnection(Util.GetConnection()))
    //    {
    //        con.Open();
    //        string query = @"DELETE FROM Orders WHERE OrderNo=@OrderNo";

    //        using (SqlCommand cmd = new SqlCommand(query, con))
    //        {
    //            cmd.Parameters.AddWithValue("@OrderNo", ID);
    //            cmd.ExecuteNonQuery();
    //            Response.Redirect("OrdersWarehouseAdmin.aspx");
    //        }
    //    }
    //}

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Purchases.aspx");
    }

    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    dpAnnouncements.SetPageProperties(0, dpAnnouncements.MaximumRows, false);

    //    if (txtKeyword.Text == "")
    //        GetAnnouncement();
    //    else
    //        GetAnnouncement(txtKeyword.Text);
    //}
}