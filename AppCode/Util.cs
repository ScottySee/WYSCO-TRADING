using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for Util
/// </summary>
public class Util
{
    public Util()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string GetConnection()
    {
        return ConfigurationManager.ConnectionStrings["MyCon"].ConnectionString;
    }

    public static string CreateSHAHash(string Phrase)
    {
        SHA512Managed HashTool = new SHA512Managed();
        Byte[] PhraseAsByte = System.Text.Encoding.UTF8.GetBytes(string.Concat(Phrase));
        Byte[] EncryptedBytes = HashTool.ComputeHash(PhraseAsByte);
        HashTool.Clear();
        return Convert.ToBase64String(EncryptedBytes);
    }

    public static string EncryptString(string strEncrypted)
    {
        byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(strEncrypted);
        string encrypted = Convert.ToBase64String(b);
        return encrypted;
    }

    public static string DecodeFrom64(string encodedData)
    {
        System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        System.Text.Decoder utf8Decode = encoder.GetDecoder();
        byte[] todecode_byte = Convert.FromBase64String(encodedData);
        int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        char[] decoded_char = new char[charCount];
        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        string result = new String(decoded_char);
        return result;
    }

    public static double GetPrice(string ID)
    {
        using (SqlConnection con = new SqlConnection(GetConnection()))
        {
            con.Open();
            string query = @"SELECT Price FROM Products WHERE ProductID=@ProductID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@ProductID", ID);
                return Convert.ToDouble((decimal)cmd.ExecuteScalar());
            }
        }
    }

    public static bool IsExisting(string ID)
    {
        using (SqlConnection con = new SqlConnection(GetConnection()))
        {
            con.Open();
            string query = @"SELECT ProductID FROM OrderDetails
                WHERE OrderNo IS NULL AND UserID=@UserID
                AND ProductID=@ProductID";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                //cmd.Parameters.AddWithValue("@OrderNo", 0);
                cmd.Parameters.AddWithValue("@UserID", HttpContext.Current.Session["UserID"].ToString());
                // HttpContext.Current.Session["userid"].ToString()
                cmd.Parameters.AddWithValue("@ProductID", ID);
                return cmd.ExecuteScalar() == null ? false : true;
            }
        }
    }

    public static void AddToCart(string ID, string quantity)
    {
        using (SqlConnection con = new SqlConnection(GetConnection()))
        {
            con.Open();
            string query = "";
            bool existingProduct = IsExisting(ID);

            if (existingProduct)
            {
                query = @"UPDATE OrderDetails SET Quantity = Quantity + @Quantity,
                    Amount = Amount + @Amount WHERE OrderNo IS NULL AND
                    UserID=@UserID AND ProductID=@ProductID";
            }
            else
            {
                query = @"INSERT INTO OrderDetails (UserID,
                    ProductID, Quantity, Amount) VALUES (@UserID,
                    @ProductID, @Quantity, @Amount)";
            }

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                string UserID = HttpContext.Current.Session["UserID"].ToString();
                //cmd.Parameters.AddWithValue("@OrderNo", null);
                cmd.Parameters.AddWithValue("@UserID", UserID);
                // HttpContext.Current.Session["userid"].ToString()
                cmd.Parameters.AddWithValue("@ProductID", ID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@Amount",
                    int.Parse(quantity) * GetPrice(ID));
                cmd.ExecuteNonQuery();

            }
        }
    }

    public static void Log(string UserID, string activity)
    {
        using (SqlConnection con = new SqlConnection(GetConnection()))
        {
            con.Open();
            string query = @"INSERT INTO Logs (UserID, LogTime, Activity)
                                VALUES (@UserID, @LogTime, @Activity)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@LogTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@Activity", EncryptString(activity));

                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void InventoryRecord(string ProductID, string quantity, string activity)
    {
        using (SqlConnection con = new SqlConnection(GetConnection()))
        {
            con.Open();
            string query = @"INSERT INTO InventoryLog (UserID, ProductID, Quantity, LogTime, Activity)
                                VALUES (@UserID, @ProductID, @Quantity, @LogTime, @Activity)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                string UserID = HttpContext.Current.Session["UserID"].ToString();
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@ProductID", ProductID);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@LogTime", DateTime.Now);
                cmd.Parameters.AddWithValue("@Activity", activity);

                cmd.ExecuteNonQuery();
            }
        }
    }

    public static void SalesRecord(string OrderNo, string Amount)
    {
        using (SqlConnection con = new SqlConnection(GetConnection()))
        {
            con.Open();
            string query = @"INSERT INTO SalesLog (UserID, OrderNo, Amount, LogTime)
                                VALUES (@UserID, @OrderNo, @Amount, @LogTime)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                string UserID = HttpContext.Current.Session["UserID"].ToString();
                cmd.Parameters.AddWithValue("@UserID", UserID);
                cmd.Parameters.AddWithValue("@OrderNo", OrderNo);
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@LogTime", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
        }
    }

    //public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
    //{
    //    string mimeType = "application/unknown";
    //    string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
    //    Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
    //    if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
    //    return mimeType;
    //}
}