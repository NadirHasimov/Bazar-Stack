using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Bazar_Stack
{
    public partial class ListOfProductsDueToDate : Form
    {
        private Form1 form1;
        const string constr = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=BazarStack;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public ListOfProductsDueToDate()
        {
            InitializeComponent();
        }

        public ListOfProductsDueToDate(Form1 form1)
        {
            this.form1 = form1;
            InitializeComponent();
        }

        private void ListOfProductsDueToDate_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
            dataGridView1.AutoSize = false;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlDataAdapter adapt = new SqlDataAdapter();
                DataSet ds = new DataSet();
                adapt.SelectCommand = new SqlCommand("uspGetListOfProductsInDate", con);
                adapt.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddToGridViewer();
        }
        public void AddToGridViewer()
        {
            dataGridView1.DataBindings.Clear();
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlDataAdapter adapt = new SqlDataAdapter();
                DataSet ds = new DataSet();
                adapt.SelectCommand = new SqlCommand("uspGetListOfProductsInDate", con);
                adapt.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
        private void Delete_Button(object sender, EventArgs e)
        {
            using(SqlConnection con=new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspDeleteDateOfSaledProducts",con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date1", SqlDbType.Date).Value = dateTimePicker1.Text;
                    cmd.Parameters.Add("@date2", SqlDbType.Date).Value = dateTimePicker2.Text; 
                    var affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Seçdiyiniz tarixdəki satılan məhsullar təmizləndi !","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    else MessageBox.Show("Seçdiyiniz tarixdə mal satılmayıb və ya tarixçə artıq silinib ! ","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            AddToGridViewer();
        }
    }
}
