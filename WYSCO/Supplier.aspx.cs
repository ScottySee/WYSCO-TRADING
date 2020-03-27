using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Supplier : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int editID = 0;

        if (!IsPostBack)
        {
            btnEdit.Visible = false;
            //FOR VIEWING THE EDIT INTERFACE
            //EDITVIEW START
            if (Request.QueryString["EditID"] != null)
            {

                bool validCompany = int.TryParse(Request.QueryString["EditID"].ToString(), out editID);

                if (validCompany)
                {
                    btnAdd.Visible = false;
                    btnEdit.Visible = true;
                    //GetCategories();
                    EditCompany(editID);
                }
                else
                    Response.Redirect("Supplier.aspx");
            }
            //EDITVIEW END
            //DELETE ID CHECKING START
            else if (Request.QueryString["DeleteID"] != null)
            {
                int DeleteID = 0;
                bool validCompany = int.TryParse(Request.QueryString["DeleteID"].ToString(), out DeleteID);

                if (validCompany)
                {
                    DeleteRecord(DeleteID);
                }
                else
                    Response.Redirect("Supplier.aspx");
            }
            //DELETE ID CHECKING END
            GetSupplier();
        }
        //FOR VIEWING
        GetSupplier();
    }

    void GetSupplier()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            string query = @"SELECT * FROM Supplier WHERE Status != 'Archived'";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Supplier");
                    lvSupplier.DataSource = ds;
                    lvSupplier.DataBind();
                }
            }
        }
    }

    protected void AddCompany(object sender, EventArgs e)
    {
        if (txtCompanyName.Text.Trim().Length > 0)
        {
            int supplierID = 0;
            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO Supplier VALUES (@CompanyName, @TinNo, @Address, @Status, @DateAdded, @DateModified)
                                    SELECT SCOPE_IDENTITY()";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", Server.HtmlEncode(txtCompanyName.Text.Trim()));
                    cmd.Parameters.AddWithValue("@TinNo", Server.HtmlEncode(txtTin.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Address", Server.HtmlEncode(txtAddress.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Status", Server.HtmlEncode("Active"));
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    //cmd.ExecuteNonQuery();
                    supplierID = Convert.ToInt32(cmd.ExecuteScalar());

                    message.InnerText = "Supplier Successfully Added.";
                }
            }

            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO Purchases (SupplierID, Amount, Status, DateAdded, DateModified)
                                    VALUES (@SupplierID, @Amount, @Status, @DateAdded, @DateModified)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@SupplierID", supplierID);
                    cmd.Parameters.AddWithValue("@Amount", Server.HtmlEncode("0.00"));
                    cmd.Parameters.AddWithValue("@Status", Server.HtmlEncode("Active"));
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    //lahat ng textbox
                    txtCompanyName.Text = "";
                    txtTin.Text = "";
                    txtAddress.Text = "";

                    Response.Redirect("Supplier.aspx");
                }
            }
        }
        else
        {
            message.InnerText = "Company Name cannot be empty.";
        }
    }

    protected void EditCompany(int ID)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string query = @"SELECT * FROM Supplier WHERE SupplierID=@SupplierID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@SupplierID", ID);
                using (SqlDataReader data = cmd.ExecuteReader())
                {
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            supplierID.Text = data["SupplierID"].ToString();
                            Session["SupplierID"] = data["SupplierID"].ToString();
                            txtCompanyName.Text = data["CompanyName"].ToString();
                            txtAddress.Text = data["Address"].ToString();
                            txtTin.Text = data["TinNo"].ToString();
                        }
                    }
                    else
                    {
                        Response.Redirect("Supplier.aspx");
                    }
                }
            }
        }
    }

    protected void SaveCompany(object sender, EventArgs e)
    {
        if (txtCompanyName.Text.Trim().Length > 0)
        {
            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Supplier SET CompanyName=@CompanyName, TinNo=@TinNo, Address=@Address,
                DateModified=@DateModified
                WHERE SupplierID=@SupplierID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", Server.HtmlEncode(txtCompanyName.Text.Trim()));
                    cmd.Parameters.AddWithValue("@TinNo", Server.HtmlEncode(txtTin.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Address", Server.HtmlEncode(txtAddress.Text.Trim()));
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@SupplierID", Session["SupplierID"].ToString());
                    cmd.ExecuteNonQuery();

                    message.InnerText = "Supplier Successfully Updated.";

                    txtCompanyName.Text = "";
                    txtTin.Text = "";
                    txtAddress.Text = "";

                    Response.Redirect("Supplier.aspx");
                }
            }

            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"UPDATE Purchases SET CompanyName=@CompanyName, TinNo=@TinNo, Address=@Address,
                WHERE PurchaseID=@PurchaseID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", Server.HtmlEncode(txtCompanyName.Text.Trim()));
                    cmd.Parameters.AddWithValue("@TinNo", Server.HtmlEncode(txtTin.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Address", Server.HtmlEncode(txtAddress.Text.Trim()));
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@PurchaseID", Session["SupplierID"].ToString());
                    cmd.ExecuteNonQuery();

                    message.InnerText = "Supplier Successfully Updated.";

                    txtCompanyName.Text = "";
                    txtTin.Text = "";
                    txtAddress.Text = "";

                    Response.Redirect("Supplier.aspx");
                }
            }
        }
        else
        {
            message.InnerText = "Company Name cannot be empty";
        }
    }

    void DeleteRecord(int ID)
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        {
            con.Open();
            string query = @"UPDATE Supplier SET Status = 'Archived' WHERE SupplierID=@SupplierID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@SupplierID", ID);
                cmd.ExecuteNonQuery();

                Response.Redirect("Supplier.aspx");
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Supplier.aspx");
    }
}