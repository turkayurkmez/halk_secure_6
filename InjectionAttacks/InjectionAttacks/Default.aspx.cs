using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\Mssqllocaldb;Initial Catalog=Northwind;Integrated Security=True");

        SqlCommand command = sqlConnection.CreateCommand();
        command.CommandText = "Select FirstName, LastName FROM Employees WHERE FirstName=@name AND LastName=@password";
        command.Parameters.AddWithValue("@name", TextBoxUserName.Text);
        command.Parameters.AddWithValue("@password", TextBoxPassword.Text);


        sqlConnection.Open();
        SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Label1.Text = "Login başarılı";
        }
        else
        {
            Label1.Text = "Login başarısız";

        }

        sqlConnection.Close();


    }

    protected void ButtonComment_Click(object sender, EventArgs e)
    {
        LabelComments.Text = TextBoxComment.Text;
    }
}