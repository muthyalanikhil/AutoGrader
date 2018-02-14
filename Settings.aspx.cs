using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Settings : System.Web.UI.Page
{
    String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["username"] != null && Session["role"].ToString() == "admin")
            {
                PopulateAdminGV();
                PopulateUsersGV();
            }
            else
            {
                Response.Redirect("~/Error");
            }
        }
    }

    private void PopulateAdminGV()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id,userName FROM user where role = 'admin'";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            adminListGV.DataSource = ds.Tables[0].DefaultView;
            adminListGV.DataBind();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    private void PopulateUsersGV()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id,userName FROM user where role = 'student'";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            allUsersListGV.DataSource = ds.Tables[0].DefaultView;
            allUsersListGV.DataBind();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    protected void moveToAdmin_Click(object sender, ImageClickEventArgs e)
    {
            ImageButton button = sender as ImageButton;
            MakeAdmin(Convert.ToInt32(button.CommandArgument));
    }


    protected void moveToStudent_Click(object sender, ImageClickEventArgs e)
    {
        int count = 0;
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT count(*) FROM user where role = 'admin'";
            count = Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
        if (count > 1)
        {
            ImageButton button = sender as ImageButton;
            MakeStudent(Convert.ToInt32(button.CommandArgument));
        }
        else
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Application should have atleast one admin.')</script>");
        }
    }

    private void MakeAdmin(int id)
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection.Open();
        try
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE user SET role = 'admin' WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error occured. Please retry.')</script>");
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                PopulateAdminGV();
                PopulateUsersGV();
            }
        }
    }

    private void MakeStudent(int id)
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection.Open();
        try
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE user SET role = 'student' WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Error occured. Please retry.')</script>");
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
                PopulateAdminGV();
                PopulateUsersGV();
            }
        }
    }
}