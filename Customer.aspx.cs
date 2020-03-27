using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Customer : System.Web.UI.Page
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
                    Response.Redirect("Customer.aspx");
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
                    Response.Redirect("Customer.aspx");
            }
            //DELETE ID CHECKING END
            GetCustomer();
        }
        //FOR VIEWING
        GetCustomer();
    }

    void GetCustomer()
    {
        using (SqlConnection con = new SqlConnection(Util.GetConnection()))
        //@"Server=MSI\MSSQLSERVER2;Database=Test;Integrated Security=true";
        {
            con.Open();
            string query = @"SELECT * FROM Customer WHERE Status != 'Archived'";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds, "Customer");
                    lvCustomer.DataSource = ds;
                    lvCustomer.DataBind();
                }
            }
        }
    }

    protected void AddCompany(object sender, EventArgs e)
    {
        if (txtCompanyName.Text.Trim().Length > 0)
        {
            int customerID = 0;
            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO Customer VALUES (@CompanyName, @TinNo, @Address, @Status, @DateAdded, @DateModified)
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
                    customerID = Convert.ToInt32(cmd.ExecuteScalar());

                    message.InnerText = "Customer Successfully Added.";
                }
            }

            using (SqlConnection con = new SqlConnection(Util.GetConnection()))
            {
                con.Open();
                string query = @"INSERT INTO Sales (CustomerID, Amount, Status, DateAdded, DateModified)
                                  VALUES (@CustomerID, @Amount, @Status, @DateAdded, @DateModified)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CustomerID", customerID);
                    cmd.Parameters.AddWithValue("@Amount", Server.HtmlEncode("0.00"));
                    cmd.Parameters.AddWithValue("@Status", Server.HtmlEncode("Active"));
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@DateModified", DBNull.Value);
                    cmd.ExecuteNonQuery();

                    message.InnerText = "Customer Successfully Added.";

                    //lahat ng textbox
                    txtCompanyName.Text = "";
                    txtTin.Text = "";
                    txtAddress.Text = "";

                    Response.Redirect("Customer.aspx");
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
            string query = @"SELECT * FROM Customer WHERE CustomerID=@CustomerID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@CustomerID", ID);
                using (SqlDataReader data = cmd.ExecuteReader())
                {
                    if (data.HasRows)
                    {
                        while (data.Read())
                        {
                            customerID.Text = data["CustomerID"].ToString();
                            Session["CustomerID"] = data["CustomerID"].ToString();
                            txtCompanyName.Text = data["CompanyName"].ToString();
                            txtAddress.Text = data["Address"].ToString();
                            txtTin.Text = data["TinNo"].ToString();
                        }
                    }
                    else
                    {
                        Response.Redirect("Customer.aspx");
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
                string query = @"UPDATE Customer SET CompanyName=@CompanyName, TinNo=@TinNo, Address=@Address,
                DateModified=@DateModified
                WHERE CustomerID=@CustomerID";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@CompanyName", Server.HtmlEncode(txtCompanyName.Text.Trim()));
                    cmd.Parameters.AddWithValue("@TinNo", Server.HtmlEncode(txtTin.Text.Trim()));
                    cmd.Parameters.AddWithValue("@Address", Server.HtmlEncode(txtAddress.Text.Trim()));
                    cmd.Parameters.AddWithValue("@DateModified", DateTime.Now);
                    cmd.Parameters.AddWithValue("@CustomerID", Session["CustomerID"].ToString());
                    cmd.ExecuteNonQuery();

                    message.InnerText = "Customer Successfully Updated.";

                    txtCompanyName.Text = "";
                    txtTin.Text = "";
                    txtAddress.Text = "";

                    Response.Redirect("Customer.aspx");
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
            string query = @"UPDATE Customer SET Status = 'Archived' WHERE CustomerID=@CustomerID";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@CustomerID", ID);
                cmd.ExecuteNonQuery();

                Response.Redirect("Customer.aspx");
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Customer.aspx");
    }

}