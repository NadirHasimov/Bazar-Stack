using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Bazar_Stack
{
    public partial class TopSalledProducts : Form
    {
        const string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BazarStock;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private Form1 form1;

        public TopSalledProducts()
        {
            InitializeComponent();
        }

        public TopSalledProducts(Form1 form1)
        {
            this.form1 = form1;
        }

        private void TopSalledProducts_Load(object sender, EventArgs e)
        {
            AddDataGrid();
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        public void AddDataGrid()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                dataGridView1.DataBindings.Clear();
                con.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();
                adapter.SelectCommand = new SqlCommand("uspGetTopSelledProducts", con);
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
    }
}
