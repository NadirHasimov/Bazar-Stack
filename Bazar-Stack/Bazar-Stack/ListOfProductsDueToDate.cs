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
        const string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BazarStock;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
    }
}
