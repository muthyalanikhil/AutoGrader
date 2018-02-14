using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Submissions : System.Web.UI.Page
{

    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string assignID = "";
    string userID = "";
    string submissionPath = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null && Session["role"].ToString() == "student")
        {
            assignID = Request.QueryString["assignmentId"];
            userID = Session["userID"].ToString();
            LoadStudentGridView();
            if (!IsPostBack)
            {
                checkAssignmentSubmission();
            }
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }

    protected void checkAssignmentSubmission()
    {
        var result = 0;
        string points = "";
        
        MySqlConnection connection1 = new MySqlConnection(connectionString);
        connection1.Open();
        try
        {
            MySqlCommand mysqlcmd = connection1.CreateCommand();
            mysqlcmd.CommandText = "SELECT count(*) FROM studentassignments WHERE assignmentId=@assignmentId and studentId=@userId";
            mysqlcmd.Parameters.AddWithValue("assignmentId", assignID);
            mysqlcmd.Parameters.AddWithValue("userId", userID);
            result = Convert.ToInt32(mysqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            //Response.Redirect("~/Error");
        }
        finally
        {
            if (connection1.State == ConnectionState.Open)
            {
                connection1.Close();
            }
        }
        if (result > 0)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            try
            {
                MySqlCommand comm = connection.CreateCommand();
                comm.CommandText = "SELECT points,filePath FROM studentassignments where assignmentId=@assignmentId and studentId=@userId";
                comm.Parameters.AddWithValue("@assignmentId", assignID);
                comm.Parameters.AddWithValue("@userId", userID);
                MySqlDataAdapter adap = new MySqlDataAdapter(comm);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                DataTable dt = ds.Tables[0];
                points = dt.Rows[0][0].ToString();
                submissionPath = dt.Rows[0][1].ToString();
                Session["submissionPath"] = submissionPath;
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
        if (points != null && submissionPath != "")
        {
            resultPH.Visible = true;
            if (points != "")
            {
                percentLBL.Text = "Percentage: " + points + "%";
            }        
            uploadAssignment.Text = "Resubmit Assignment";           
       }
    }

    protected void LoadStudentGridView()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT assignmentId,assignmentName,assignmentDescription FROM assignments where assignmentId=@assignmentId";
            cmd.Parameters.AddWithValue("@assignmentId", assignID);
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            studentGridView.DataSource = ds.Tables[0].DefaultView;
            studentGridView.DataBind();
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

    protected void DownloadZip(object sender, EventArgs e)
    {
        string path = Session["submissionPath"].ToString();
        FileInfo file = new FileInfo(path);
        if (file.Exists)
        {
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=Assignment.zip");
            Response.AddHeader("Content-Length", file.Length.ToString());
            Response.ContentType = "application/x-zip-compressed";
            Response.WriteFile(file.FullName);
            Response.Flush();
            Response.End();
        }
        else
        {
            ClientScript.RegisterStartupScript(Type.GetType("System.String"), "messagebox", "&lt;script type=\"text/javascript\"&gt;alert('File not Found');</script>");
        }
    }

    protected void submitAssignment_Click(object sender, EventArgs e)
    {
        var result = 0;
        MySqlConnection connection1 = new MySqlConnection(connectionString);
        connection1.Open();
        try
        {
            MySqlCommand mysqlcmd = connection1.CreateCommand();
            mysqlcmd.CommandText = "SELECT count(*) FROM studentassignments WHERE assignmentId=@assignmentId and studentId=@userId";
            mysqlcmd.Parameters.AddWithValue("assignmentId", assignID);
            mysqlcmd.Parameters.AddWithValue("userId", userID);
            result = Convert.ToInt32(mysqlcmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            
        }
        finally
        {
            if (connection1.State == ConnectionState.Open)
            {
                connection1.Close();
            }
        }

        if (result > 0)
        {
            if (uploadZIPButton.HasFile)
            {
                string path = Server.MapPath("~/Uploads/" + userID + "_" + assignID + ".zip");
                uploadZIPButton.SaveAs(path);
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd;
                connection.Open();
                try
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "UPDATE studentassignments SET points=@points,comments=@comments,zipFileName=@zipFileName,filePath=@filePath WHERE assignmentId=@assignmentId and studentId=@userId";
                    cmd.Parameters.AddWithValue("@assignmentId", assignID);
                    cmd.Parameters.AddWithValue("@userId", userID);
                    cmd.Parameters.AddWithValue("@points", "");
                    cmd.Parameters.AddWithValue("@comments", "resubmitted");
                    cmd.Parameters.AddWithValue("@zipFileName", userID + "_" + assignID + ".zip");
                    cmd.Parameters.AddWithValue("@filePath", path);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Submission Failed. Please retry.')</script>");
                    Response.Redirect("~/Error");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        ClientScript.RegisterStartupScript(Page.GetType(), "Success", "<script language='javascript'>alert('Assignment re-submitted successfully.')</script>");
                        Response.Redirect("~/StudentView", false);
                    }
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose a file to upload')</script>");
            }
        }
        else {
            if (uploadZIPButton.HasFile)
            {
                string path = Server.MapPath("~/Uploads/" + userID + "_" + assignID + ".zip");
                uploadZIPButton.SaveAs(path);
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand cmd;
                connection.Open();
                try
                {
                    cmd = connection.CreateCommand();
                    cmd.CommandText = "INSERT INTO studentassignments(assignmentId,studentId,points,comments,zipFileName,filePath) VALUES(@assignmentId,@studentId,@points,@comments,@zipFileName,@filePath)";
                    cmd.Parameters.AddWithValue("@assignmentID", assignID);
                    cmd.Parameters.AddWithValue("@studentId", userID);
                    cmd.Parameters.AddWithValue("@points", "");
                    cmd.Parameters.AddWithValue("@comments", "submitted");
                    cmd.Parameters.AddWithValue("@zipFileName", userID + "_" + assignID + ".zip");
                    cmd.Parameters.AddWithValue("@filePath", path);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "Error", "<script language='javascript'>alert('Submission Failed. Please retry.')</script>");
                    Response.Redirect("~/Error");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        ClientScript.RegisterStartupScript(Page.GetType(), "Success", "<script language='javascript'>alert('Assignment submitted successfully.')</script>");
                        Response.Redirect("~/StudentView", false);
                    }
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please choose a file to upload')</script>");
            }
        }      
    }
}