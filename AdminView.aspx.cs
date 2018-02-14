using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;

public partial class AdminView : Page
{
    String connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null && Session["role"].ToString() == "admin")
        {
            maxfilesize.Visible = false;
            if (!IsPostBack)
            {
                LoadGridData();
            }           
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }

    protected void createAssignment_Click(object sender, EventArgs e)
    {
        string assignName = assignmentName.Text;
        string assignDesc = assignmentDescription.Text;
        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection.Open();
        ////
        try
        {
            string filename = Path.GetFileName(fileUploadButton.PostedFile.FileName);
            string contentType = fileUploadButton.PostedFile.ContentType;
            int fileSize = fileUploadButton.PostedFile.ContentLength;
            if (fileSize > (50 * 1024))
            {
                maxfilesize.Visible = true;
                maxfilesize.Text = "Filesize is too large. Maximum file size permitted is " + 50 + "KB";
                return;
            }
            using (Stream fs = fileUploadButton.PostedFile.InputStream)
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO assignments(createdBy,assignmentName,assignmentDescription,assignmentCreatedAt,Data,contentType) VALUES(@createdBy,@assignmentName,@assignmentDescription,@assignmentCreatedAt,@Data,@contentType)";
                    cmd.Parameters.AddWithValue("@createdBy", Session["username"].ToString());
                    cmd.Parameters.AddWithValue("@assignmentName", assignName);
                    cmd.Parameters.AddWithValue("@assignmentDescription", assignDesc);
                    cmd.Parameters.AddWithValue("@assignmentCreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Data", bytes);
                    cmd.Parameters.AddWithValue("@contentType", contentType);
                    cmd.ExecuteNonQuery();

                }
            }
            ClientScript.RegisterStartupScript(Page.GetType(), "Success", "<script language='javascript'>alert('Assignment successfully created. Please upload the testcases in assignments page.')</script>");
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
                assignmentName.Text = "";
                assignmentDescription.Text = "";
                fileUploadButton.Attributes.Clear();
                LoadGridData();
            }
        }
    }

    protected void DownloadFile(object sender, EventArgs e)
    {
        int id = int.Parse((sender as LinkButton).CommandArgument);
        byte[] bytes;
        string fileName, contentType;
        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "select assignmentName, Data, ContentType from assignments where assignmentId=@Id";
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Connection = con;
                con.Open();
                using (MySqlDataReader sdr = cmd.ExecuteReader())
                {
                    sdr.Read();
                    bytes = (byte[])sdr["Data"];
                    contentType = sdr["ContentType"].ToString();
                    fileName = sdr["assignmentName"].ToString();
                }
                con.Close();
            }
        }
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "";
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.ContentType = contentType;
        if (ContentType != null)
        {
            switch (ContentType.ToLower())
            {
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    fileName = fileName + ".docx";
                    break;
                case ".html":
                case "text/html":
                    fileName = fileName + ".html";
                    break;
                case ".txt":
                case "text/plain":
                    fileName = fileName + ".txt";
                    break;
                case ".doc":
                case ".rtf":
                case "application/msword":
                    fileName = fileName + ".doc";
                    break;

                case ".xls":
                    fileName = fileName + ".xls";
                    break;
            }
        }
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.BinaryWrite(bytes);
        Response.Flush();
        Response.End();
    }

    private void LoadGridData()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM assignments";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            assignmentsGridView.DataSource = ds.Tables[0].DefaultView;
            assignmentsGridView.DataBind();
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

    protected void deleteAssignment_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int rowIndex = gvr.RowIndex; // Get the current row
        string assignmentID = assignmentsGridView.Rows[rowIndex].Cells[0].Text;

        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmdd;
        connection.Open();
        try
        {
            cmdd = connection.CreateCommand();
            cmdd.CommandText = "DELETE FROM assignments WHERE assignmentId=@assignId";
            cmdd.Parameters.AddWithValue("@assignId", assignmentID);
            cmdd.ExecuteNonQuery();
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

        MySqlConnection connection1 = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection1.Open();
        try
        {
            cmd = connection1.CreateCommand();
            cmd.CommandText = "DELETE FROM studentassignments WHERE assignmentId=@assignId";
            cmd.Parameters.AddWithValue("@assignId", assignmentID);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection1.State == ConnectionState.Open)
            {
                connection1.Close();
            }
        }

        MySqlConnection connection2 = new MySqlConnection(connectionString);
        MySqlCommand cm;
        connection2.Open();
        try
        {
            cm = connection2.CreateCommand();
            cm.CommandText = "DELETE FROM testcases WHERE assignmentId=@assignId";
            cm.Parameters.AddWithValue("@assignId", assignmentID);
            cm.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Response.Redirect("~/Error");
        }
        finally
        {
            if (connection2.State == ConnectionState.Open)
            {
                connection2.Close();
            }
        }

        LoadGridData();
    }
}