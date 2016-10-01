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
            dataGridView1.DataBindings.Clear();
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
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            textBox7.ReadOnly = true;
            MinimizeBox = true;
            MaximizeBox = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = 0;
            a = checkList(e.ColumnIndex);

            if (a != 0 && e.RowIndex == -1)
            {

            }
            else
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
        }
        private void AddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd1 = new SqlCommand("uspCheckProductName", con))
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add("@Name", SqlDbType.NVarChar).Value = textBox1.Text;
                        int check = (int)(cmd1.ExecuteScalar() ?? 0);
                        if (check != 0)
                        {
                            MessageBox.Show("Bu məhsul artıq yüklənib !");
                        }
                        else
                        {
                            using (SqlCommand cmd = new SqlCommand("uspInsertProduct", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = textBox1.Text.ToString();
                                cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox2.Text);
                                cmd.Parameters.Add("@PriceOfProduct ", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                                cmd.Parameters.Add("@Count", SqlDbType.Int).Value = Convert.ToInt32(textBox8.Text);
                                cmd.Parameters.Add("@Date", SqlDbType.Date).Value = DateTime.Now;
                                try
                                {
                                    var affectedRows = cmd.ExecuteNonQuery();
                                    if (affectedRows < 1)
                                    {
                                        MessageBox.Show("Əməliyyat alınmadı! Yenidən cəhd edin!");
                                    }
                                    else MessageBox.Show("Məhsul uğurla yükləndi !");
                                }
                                catch (System.Data.SqlClient.SqlException)
                                {
                                    MessageBox.Show("Məhsulun sayı mənfi ola bilməz!");
                                }
                            }
                        }
                    }
                    DataSet ds = new DataSet();
                    ds.Clear();

                }
                AddToGridView();
                dataGridView1.Refresh();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Zəhmət olmasa əlavə edəcəyiniz məhsulun parametrlərini doldurun !");
            }
        }
        private void Update_Products_Button_Click(object sender, EventArgs e)
        {
            try
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
                        try
                        {

                            cmd.Parameters.Add("@Count", SqlDbType.Int).Value = int.Parse(textBox8.Text) + int.Parse(textBox4.Text);
                            var affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows < 1)
                            {
                                MessageBox.Show("Əməliyyat uğurla yerinə yetirilmədi!");
                            }
                            else MessageBox.Show("Seçdiyiniz məhsulun parametrləri yeniləndi!");
                        }
                        catch (System.FormatException) { MessageBox.Show("Dəyişdirəcəyiniz məhsulun sayını daxil edin !"); }


                    }
                }
                AddToGridView();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Dəyişdirəcəyiniz məhsulu seçin !");
            }
        }

        private void SaleBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("uspSaleProduct", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(textBox5.Text);
                        cmd.Parameters.Add("@Count", SqlDbType.Int).Value = int.Parse(textBox8.Text);
                        try
                        {
                            var affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows < 1)
                            {
                                MessageBox.Show("Əməliyyat  yerinə yetirilə bilmədi !");
                            }
                            else MessageBox.Show("Əməliyyat uğurla yerinə yetirldi !");

                        }
                        catch (System.Data.SqlClient.SqlException)
                        {
                            MessageBox.Show("Satacağınız məhsulun sayı bazadakından artıqdır !");
                        }
                    }
                }
                AddToGridView();

            }
            catch (System.FormatException)
            {
                MessageBox.Show("Satacağınız məhsulu seçin!");
            }
        }

        private void DeletButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("uspDeleteProducts", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = int.Parse(textBox5.Text);
                        var affectedRows = cmd.ExecuteNonQuery();
                        if (affectedRows < 1)
                        {
                            MessageBox.Show("Əməliyyat yerinə yetirilə bilmədi !");
                        }
                        else MessageBox.Show("Əməliyyat uğurla yerinə yetirildi !");
                    }
                    AddToGridView();
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Siləcəyiniz məhsulu seçin !");
            }
        }
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int a = 0;
            a = checkList(e.ColumnIndex);
            if (a == 0) { a = 11; }
            if (a != 0 && e.RowIndex == -1)
            {

            }
            else
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
                try
                {

                    textBox7.Text = "" + (decimal.Parse(textBox3.Text) - decimal.Parse(textBox2.Text)) * decimal.Parse(textBox4.Text);
                }
                catch (System.FormatException)
                {

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form frm = new TopSalledProducts();
            frm.Visible = true;
        }
        public int checkList(int number)
        {
            listOfNumbers.Add(-1);
            listOfNumbers.Add(0);
            listOfNumbers.Add(1);
            listOfNumbers.Add(2);
            listOfNumbers.Add(3);
            listOfNumbers.Add(4);
            listOfNumbers.Add(5);
            listOfNumbers.Add(6);
            return listOfNumbers.Find(x => x == number);
        }
        public List<int> listOfNumbers = new List<int>();

        private void Gain_Button_Click(object sender, EventArgs e)
        {
            Form frm = new BenefitInDate();
            frm.Visible = true;
        }
        class Benefit
        {
            public decimal Price { get; set; }
            public decimal PriceOfProduct { get; set; }
            public int CountOfSold { get; set; }
        }
    }
}
