using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using DBLayer;

namespace DataTableSample
{
    public partial class Default : System.Web.UI.Page
    {
        DBLayer.DBLayer dbl = new DBLayer.DBLayer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// Binds the grid with rs
        /// </summary>
        private void BindGrid()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnHus"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Hus", conn);
                cmd.CommandType = CommandType.Text;
                SqlDataReader reader = cmd.ExecuteReader();

                dt.Load(reader);

                reader.Close();
                conn.Close();
            }
            GridViewBoligEiere.DataSource = dt;
            GridViewBoligEiere.DataBind();
        }

        protected void ButtonSearchByPhone_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = dbl.GetBoligAndOwnersByPhone(TextBoxSearchByPhone.Text);
            GridView1.DataBind();
        }

        protected void GridViewBoligEiere_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            GridViewBoligEiere.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void GridViewBoligEiere_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            SqlParameter param;

            Label HusID = GridViewBoligEiere.Rows[e.RowIndex].FindControl("lbl_HusID") as Label;
            TextBox Energimerking = GridViewBoligEiere.Rows[e.RowIndex].FindControl("txt_Energimerking") as TextBox;

            var connectionString = ConfigurationManager.ConnectionStrings["ConnHus"].ConnectionString;
            DataTable dt = new DataTable();
           
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Hus SET Energimerking = @Energimerking WHERE HusID = @HusID", conn);
                cmd.CommandType = CommandType.Text;

                param = new SqlParameter("@HusID", SqlDbType.Int);
                param.Value = int.Parse(HusID.Text);
                cmd.Parameters.Add(param);

                param = new SqlParameter("@Energimerking", SqlDbType.NVarChar);
                param.Value = Energimerking.Text;
                cmd.Parameters.Add(param);

                SqlDataReader reader = cmd.ExecuteReader();
                reader.Close();
                conn.Close();
            }

            GridViewBoligEiere.EditIndex = -1;
            GridViewBoligEiere.DataSource = dbl.GetAllBolig();
            GridViewBoligEiere.DataBind(); 
        }

        protected void GridViewBoligEiere_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            GridViewBoligEiere.EditIndex = -1;

            GridViewBoligEiere.DataSource = dbl.GetAllBolig();
            GridViewBoligEiere.DataBind();
        }
    }
} 