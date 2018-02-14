using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;
using AutoGrader;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

public partial class Register : Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void CreateUser_Click(object sender, EventArgs e)
    {
        string username = UserName.Text;
        string pass = Password.Text;
        string role = "student";
        var password = "";
        if (pass != null && pass != "")
        {
            SHA1CryptoServiceProvider encrypt = new SHA1CryptoServiceProvider();
            byte[] encryptText = encrypt.ComputeHash(Encoding.Default.GetBytes(pass));
            foreach (byte tempData in encryptText)
            {
                password = password + tempData.ToString();
            }
        }

        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection.Open();
        try
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO user(userName,password,role) VALUES(@userName,@password,@role)";
            cmd.Parameters.AddWithValue("@userName", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@role", role);
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            Response.Redirect("~/Error");
        }
        finally {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        Response.Redirect("~/Login");
    }
}