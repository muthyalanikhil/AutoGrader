using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Grader : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
    string assignID = "";
    string userID = "";
    string studentID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["username"] != null && Session["role"].ToString() == "admin")
        {
            assignID = Request.QueryString["assignmentId"];
            userID = Session["userID"].ToString();
            if (!Page.IsPostBack)
            {
                assignID = Request.QueryString["assignmentId"];
                userID = Session["userID"].ToString();
                LoadStudentGridView();
            }
        }
        else
        {
            Response.Redirect("~/Error");
        }
    }

    private void LoadStudentGridView()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select u.userName, a.assignmentName, u.id, s.assignmentId, s.points, s.comments, s.zipFileName, s.filePath From assignments a join studentassignments s on a.assignmentId = s.assignmentId join user u on u.Id = s.studentId where s.assignmentId = @assignmentId";
            cmd.Parameters.AddWithValue("@assignmentId", assignID);
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            if (ds.Tables[0].Rows.Count == 0)
            {
                noSubmissionsLBL.Text = "There are no submissions yet.";
            }
            else
            {
                noSubmissionsLBL.Text = "";
                studentGridView.DataSource = ds.Tables[0].DefaultView;
                studentGridView.DataBind();
            }
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

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell statusCell = e.Row.Cells[4];
            if (statusCell.Text != "" && statusCell.Text != null && statusCell.Text != "&nbsp;")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Button btnButton = (Button)e.Row.FindControl("lnkDownload");
                    btnButton.Text = "Re-Grade";
                }

            }
            else {
                statusCell.Text = "Not yet graded";
            }
        }
    }

    protected void GradeAssignment(object sender, EventArgs e)
    {
        //Get the button that raised the event
        Button btn = (Button)sender;
        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int rowIndex = gvr.RowIndex; // Get the current row
        string assignmentID = studentGridView.Rows[rowIndex].Cells[0].Text;
        string userId = studentGridView.Rows[rowIndex].Cells[1].Text;
        string userName = studentGridView.Rows[rowIndex].Cells[2].Text;
        string assignmentName = studentGridView.Rows[rowIndex].Cells[3].Text;

        string si1 = null, so1 = null, si2 = null, so2 = null, si3 = null, so3 = null;
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        try
        {
            MySqlCommand comm = connection.CreateCommand();
            comm.CommandText = "SELECT assignmentId,sampleInput1,sampleOutput1,sampleInput2,sampleOutput2,sampleInput3,sampleOutput3 FROM testcases where assignmentId=@assignmentId";
            comm.Parameters.AddWithValue("@assignmentId", assignmentID);
            MySqlDataAdapter adap = new MySqlDataAdapter(comm);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                si1 = dt.Rows[0][1].ToString();
                so1 = dt.Rows[0][2].ToString();
                si2 = dt.Rows[0][3].ToString();
                so2 = dt.Rows[0][4].ToString();
                si3 = dt.Rows[0][5].ToString();
                so3 = dt.Rows[0][6].ToString();
            }          
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

        double sum = 0;
        int numberOfTestcases = 0;
        testCasePH1.Visible = false;
        testCasePH2.Visible = false;
        testCasePH3.Visible = false;

        if (si1 != null && si1 != "")
        {
            testCasePH1.Visible = true;
            string exeOutput = CheckAssignmentZip(userId, assignmentID, assignmentName, ToStream(si1));
            if (exeOutput == so1)
            {
                output1.Text = "100%";
                sum = sum + 100;
                numberOfTestcases++;
            }
            else
            {
                output1.Text = "test case failed..!!";
                sum = sum + 0;
                numberOfTestcases++;
            }
            si1Link.Attributes.Add("data-content", si1);
            so1Link.Attributes.Add("data-content", so1);
            actualOutput1.Attributes.Add("data-content", exeOutput);
        }
        if (si2 != null && si2 != "")
        {
            testCasePH2.Visible = true;
            string exeOutput = CheckAssignmentZip(userId, assignmentID, assignmentName, ToStream(si2));
            if (exeOutput == so2)
            {
                output2.Text = "100%";
                sum = sum + 100;
                numberOfTestcases++;
            }
            else
            {
                output2.Text = "test case failed..!!";
                sum = sum + 0;
                numberOfTestcases++;
            }
            si2Link.Attributes.Add("data-content", si2);
            so2Link.Attributes.Add("data-content", so2);
            actualOutput2.Attributes.Add("data-content", exeOutput);
        }
        if (si3 != null && si3 != "")
        {
            testCasePH3.Visible = true;
            string exeOutput = CheckAssignmentZip(userId, assignmentID, assignmentName, ToStream(si3));
            if (exeOutput == so3)
            {
                output3.Text = "100%";
                sum = sum + 100;
                numberOfTestcases++;
            }
            else
            {
                output3.Text = "test case failed..!!";
                sum = sum + 0;
                numberOfTestcases++;
            }
            si3Link.Attributes.Add("data-content", si3);
            so3Link.Attributes.Add("data-content", so3);
            actualOutput3.Attributes.Add("data-content", exeOutput);
        }
        if (sum != 0 || numberOfTestcases != 0)
        {
            double percentage = sum / numberOfTestcases;
            percentLBL.Text = percentage.ToString();
            outputPH.Visible = true;
            MySqlConnection connection1 = new MySqlConnection(connectionString);
            MySqlCommand cmdd;
            connection1.Open();
            try
            {
                cmdd = connection1.CreateCommand();
                cmdd.CommandText = "UPDATE studentassignments SET points=@points WHERE assignmentId=@assignId and studentId=@userId";
                cmdd.Parameters.AddWithValue("@points", percentage);
                cmdd.Parameters.AddWithValue("@assignId", assignID);
                cmdd.Parameters.AddWithValue("@userId", userId);
                cmdd.ExecuteNonQuery();
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
                    LoadStudentGridView();
                }
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('Please upload the testcases before grading.')</script>");
        }
    }

    protected string CheckAssignmentZip(string userID, string assignID, string assignmentName, Stream inputStream)
    {
        string path = Server.MapPath("~/Uploads/" + userID + "_" + assignID + ".zip");
        string fileName = userID + "_" + assignID + ".zip";
        string finalOutput = "";

        if (File.Exists(path))
        {

            ZipFile.ExtractToDirectory(path, path.Replace(".zip", ""));
            // string unZipPath = Server.MapPath("~/Uploads/" + userID + "_" + assignID + "/" + assignmentName);
            string unZipPath = Server.MapPath("~/Uploads/" + userID + "_" + assignID);
            string subFolderName = assignmentName;
            DirectoryInfo folder = new DirectoryInfo(unZipPath);
            DirectoryInfo[] subfolders = folder.GetDirectories();
            if (subfolders.Length > 0)
            {
                subFolderName = subfolders[0].Name;
            }

            string destinationDirectory = path.Replace(".zip", "") + @"\" + subFolderName + @"\dist";

            if (Directory.Exists(destinationDirectory))
            {
                string jarName = assignmentName;
                CopyStream(inputStream, destinationDirectory + @"\input.txt");
                DirectoryInfo folder1 = new DirectoryInfo(destinationDirectory);
                FileInfo[] files = folder1.GetFiles();

                foreach (var item in files)
                {
                    if (item.Name.Contains(".jar"))
                    {
                        jarName = item.Name;
                    }
                }

                string output = ExecuteJAR(path.Replace(".zip", "") + @"\" + subFolderName + @"\dist", jarName);
                output = output.Remove(output.TrimEnd().LastIndexOf(Environment.NewLine));
                finalOutput = DeleteLines(output, 8).Trim();
                finalOutput.ToString().Trim();
                if (Directory.Exists(path.Replace(".zip", "")))
                {
                    DeleteFilesAndFoldersRecursively(path.Replace(".zip", ""));
                }
            }
            else
            {
                return "Compilation Error";
            }
        }
        return finalOutput;
    }

    public void CopyStream(Stream stream, string destPath)
    {
        using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
        {
            stream.CopyTo(fileStream);
        }
    }

    public string DeleteLines(string s, int linesToRemove)
    {
        return s.Split(Environment.NewLine.ToCharArray(),
                       linesToRemove + 1
            ).Skip(linesToRemove)
            .FirstOrDefault();
    }

    protected string ExecuteJAR(String filePath, String filename)
    {
        String output = "";
        String command = "java -jar " + filename;
        Process process = new Process();
        try
        {
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.WorkingDirectory = filePath;
            processInfo.FileName = "cmd.exe";
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            process.StartInfo = processInfo;
            process = Process.Start(processInfo);
            process.StandardInput.AutoFlush = true;
            process.StandardInput.WriteLine(command);
            process.WaitForExit(3000);

            if (!process.HasExited)
            {
                process.Kill();
            }
            output = process.StandardOutput.ReadToEnd();
        }
        catch (Exception ex)
        {
            output = ex.ToString();
        }
        finally
        {
            if (process != null)
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
                process.Dispose();
            }
        }
        return output;
    }

    public void DeleteFilesAndFoldersRecursively(string target_dir)
    {
        foreach (string file in Directory.GetFiles(target_dir))
        {
            File.Delete(file);
        }

        foreach (string subDir in Directory.GetDirectories(target_dir))
        {
            DeleteFilesAndFoldersRecursively(subDir);
        }

        Thread.Sleep(1); // This makes the difference between whether it works or not. Sleep(0) is not enough.
        Directory.Delete(target_dir);
    }

    protected Stream ToStream(string data)
    {
        // convert string to stream
        byte[] byteArray = Encoding.UTF8.GetBytes(data);
        MemoryStream stream = new MemoryStream(byteArray);
        return stream;
    }

    protected void updateTestCases_Click(object sender, EventArgs e)
    {
        string si1 = null, so1 = null, si2 = null, so2 = null, si3 = null, so3 = null;
        if (sampleInputFU1.HasFile && sampleOutputFU1.HasFile)
        {
            StreamReader readersi1 = new StreamReader(sampleInputFU1.PostedFile.InputStream);
            si1 = readersi1.ReadToEnd();
            StreamReader readerso1 = new StreamReader(sampleOutputFU1.PostedFile.InputStream);
            so1 = readerso1.ReadToEnd();
        }
        if (sampleInputFU2.HasFile && sampleOutputFU2.HasFile)
        {
            StreamReader readersi2 = new StreamReader(sampleInputFU2.PostedFile.InputStream);
            si2 = readersi2.ReadToEnd();
            StreamReader readerso2 = new StreamReader(sampleOutputFU2.PostedFile.InputStream);
            so2 = readerso2.ReadToEnd();
        }
        if (sampleInputFU3.HasFile && sampleOutputFU3.HasFile)
        {
            StreamReader readersi3 = new StreamReader(sampleInputFU3.PostedFile.InputStream);
            si3 = readersi3.ReadToEnd();
            StreamReader readerso3 = new StreamReader(sampleOutputFU3.PostedFile.InputStream);
            so3 = readerso3.ReadToEnd();
        }

        var result = 0;
        MySqlConnection connection1 = new MySqlConnection(connectionString);
        connection1.Open();
        try
        {
            MySqlCommand mysqlcmd = connection1.CreateCommand();
            mysqlcmd.CommandText = "SELECT count(*) FROM testcases WHERE assignmentId=@assignmentId";
            mysqlcmd.Parameters.AddWithValue("assignmentId", assignID);
            result = Convert.ToInt32(mysqlcmd.ExecuteScalar());
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

        if (result <= 0)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            connection.Open();
            ////
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO testcases(assignmentId,sampleInput1,sampleOutput1,sampleInput2,sampleOutput2,sampleInput3,sampleOutput3) VALUES(@assignId,@si1,@so1,@si2,@so2,@si3,@so3)";
                cmd.Parameters.AddWithValue("@assignId", assignID);
                cmd.Parameters.AddWithValue("@si1", si1);
                cmd.Parameters.AddWithValue("@so1", so1);
                cmd.Parameters.AddWithValue("@si2", si2);
                cmd.Parameters.AddWithValue("@so2", so2);
                cmd.Parameters.AddWithValue("@si3", si3);
                cmd.Parameters.AddWithValue("@so3", so3);
                cmd.ExecuteNonQuery();
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
        else
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand cmd;
            connection.Open();
            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "UPDATE testcases SET sampleInput1=@si1,sampleOutput1=@so1,sampleInput2=@si2,sampleOutput2=@so2,sampleInput3=@si3,sampleOutput3=@so3 WHERE assignmentId=@assignId";
                cmd.Parameters.AddWithValue("@assignId", assignID);
                cmd.Parameters.AddWithValue("@si1", si1);
                cmd.Parameters.AddWithValue("@so1", so1);
                cmd.Parameters.AddWithValue("@si2", si2);
                cmd.Parameters.AddWithValue("@so2", so2);
                cmd.Parameters.AddWithValue("@si3", si3);
                cmd.Parameters.AddWithValue("@so3", so3);
                cmd.ExecuteNonQuery();
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
    }

    protected void partialGrade_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int rowIndex = gvr.RowIndex; // Get the current row
        string assignmentID = studentGridView.Rows[rowIndex].Cells[0].Text;
        string userID = studentGridView.Rows[rowIndex].Cells[1].Text;
        string userName = studentGridView.Rows[rowIndex].Cells[2].Text;
        string assignmentName = studentGridView.Rows[rowIndex].Cells[3].Text;
        string points = studentGridView.Rows[rowIndex].Cells[4].Text;
        Session["studentID"] = userID;

        nameLBL.Text = "Name: " + userName;
        assignmentNameLBL.Text = "Assignment Name: " + assignmentName;
        marksTB.Text = points;

        partialGradePH.Visible = true;
    }

    protected void updateGrade_Click(object sender, EventArgs e)
    {

        string points = marksTB.Text;
        MySqlConnection connection = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        connection.Open();
        try
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE studentassignments SET points=@points WHERE assignmentId=@assignId and studentId=@userId";
            cmd.Parameters.AddWithValue("@assignId", assignID);
            cmd.Parameters.AddWithValue("@userId", Session["studentID"].ToString());
            cmd.Parameters.AddWithValue("@points", points);
            cmd.ExecuteNonQuery();
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
            Session["studentID"] = "";
            partialGradePH.Visible = false;
            LoadStudentGridView();
        }
    }
}