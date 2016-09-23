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
    public partial class Form1 : Form
    {
        const string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BazarStock;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public Form1()
        {
            InitializeComponent();
        }
        public void AddToGridView()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                SqlDataAdapter adapt = new SqlDataAdapter();
                DataSet ds = new DataSet();
                adapt.SelectCommand = new SqlCommand("uspGetAllProducts", con);
                adapt.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AddToGridView();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex;
            rowIndex = e.RowIndex;
            textBox1.DataBindings.Clear();
            textBox1.Text = "" + dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
            textBox2.DataBindings.Clear();
            textBox2.Text = "" + dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            textBox3.DataBindings.Clear();
            textBox3.Text = "" + dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            textBox4.DataBindings.Clear();
            textBox4.Text = "" + dataGridView1.Rows[rowIndex].Cells[4].Value.ToString();
            textBox5.DataBindings.Clear();
            textBox5.Text = "" + dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
        }

        private void AddProduct_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                DataSet ds = new DataSet();
                ds.Clear();
                using (SqlCommand cmd = new SqlCommand("uspInsertProduct", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = textBox1.Text.ToString();
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox2.Text);
                    cmd.Parameters.Add("@PriceOfProduct ", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                    cmd.Parameters.Add("@Count", SqlDbType.Int).Value = Convert.ToInt32(textBox4.Text);
                    var affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows < 1)
                    {
                        MessageBox.Show("Əməliyyat alınmadı! Yenidən cəhd edin!");
                    }
                    else MessageBox.Show("Məhsul uğurla yükləndi !");
                }
            }
            dataGridView1.DataBindings.Clear();
            AddToGridView();
            dataGridView1.Refresh();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspUpdateProducTable", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(textBox5.Text);
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar).Value = textBox1.Text;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = decimal.Parse(textBox2.Text);
                    cmd.Parameters.Add("@PriceOfProduct", SqlDbType.Decimal).Value = int.Parse(textBox3.Text);
                    cmd.Parameters.Add("@Count", SqlDbType.Int).Value = int.Parse(textBox4.Text);
                    var affectedRows = cmd.ExecuteNonQuery();
                    if (affectedRows < 1)
                    {
                        MessageBox.Show("Əməliyyat uğurla yerinə yetirilmədi!");
                    }
                    else MessageBox.Show("Seçdiyiniz məhsulun parametrləri yeniləndi!");
                }
                dataGridView1.DataBindings.Clear();
                AddToGridView();
            }
        }
    }
}
