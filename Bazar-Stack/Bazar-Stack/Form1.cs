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
using System.IO;
using System.Reflection;


namespace Bazar_Stack
{

    public partial class Form1 : Form
    {
        const string constr = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=BazarStack;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //const string constr = @"C:\Users\hasim\Source\Repos\Bazar-Stack\Bazar-Stack\DBCustomAction1\masterDataSet.xsd";

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
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.ColumnHeader);
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
            else if (a == 0 && e.RowIndex == -1)
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
                        if (decimal.Parse(textBox2.Text) <= decimal.Parse(textBox3.Text))
                        {
                            if (check != 0)
                            {
                                MessageBox.Show("Bu məhsul artıq yüklənib !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            }
                            else
                            {
                                using (SqlCommand cmd = new SqlCommand("uspInsertProduct", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar).Value = textBox1.Text.ToString();
                                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox2.Text.ToString());
                                    cmd.Parameters.Add("@PriceOfProduct ", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox3.Text.ToString());
                                    cmd.Parameters.Add("@Count", SqlDbType.Int).Value = Convert.ToInt32(textBox8.Text);
                                    cmd.Parameters.Add("@Date", SqlDbType.Date).Value = DateTime.Now;
                                    try
                                    {
                                        var affectedRows = cmd.ExecuteNonQuery();
                                        if (affectedRows < 1)
                                        {
                                            MessageBox.Show("Əməliyyat alınmadı! Yenidən cəhd edin!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            MessageBox.Show("Məhsul uğurla yükləndi !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            textBox8.Text = "0";
                                        }
                                    }
                                    catch (System.Data.SqlClient.SqlException)
                                    {
                                        MessageBox.Show("Məhsulun sayı mənfi və ya 0-dır!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Alis qiyməti Satis qiymətindən böyükdür !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Zəhmət olmasa əlavə edəcəyiniz məhsulun parametrlərini doldurun !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void Update_Products_Button_Click(object sender, EventArgs e)
        {
            try
            {

                if (int.Parse(textBox8.Text) > 0)
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
                                cmd.Parameters.Add("@PriceOfProduct", SqlDbType.Decimal).Value = decimal.Parse(textBox3.Text);
                                try
                                {
                                    cmd.Parameters.Add("@Count", SqlDbType.Int).Value = int.Parse(textBox8.Text) + int.Parse(textBox4.Text);
                                    try
                                    {
                                        var affectedRows = cmd.ExecuteNonQuery();
                                        if (affectedRows < 1)
                                        {
                                            MessageBox.Show("Əməliyyat uğurla yerinə yetirilmədi!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else MessageBox.Show("Seçdiyiniz məhsulun parametrləri yeniləndi!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    }
                                    catch (System.FormatException)
                                    {
                                        MessageBox.Show("Sayı düzgün daxil edin !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch (System.Data.SqlClient.SqlException)
                                    {
                                        MessageBox.Show("Say mənfi ola bilməz !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                catch (System.FormatException) { MessageBox.Show("Dəyişdirəcəyiniz məhsulun sayını daxil edin !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                        AddToGridView();
                    }
                    catch (System.FormatException)
                    {
                        MessageBox.Show("Dəyişdirəcəyiniz məhsulu seçin !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Say 0 və ya mənfi ola bilməz !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Sayı düzgün daxil edin !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@Date2", SqlDbType.DateTime).Value = DateTime.Now.ToString("H:mm:ss");
                        cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox2.Text.ToString());
                        cmd.Parameters.Add("@PriceOfProduct", SqlDbType.Decimal).Value = Convert.ToDecimal(textBox3.Text.ToString());
                        try
                        {
                            var affectedRows = cmd.ExecuteNonQuery();
                            if (affectedRows < 1)
                            {
                                MessageBox.Show("Əməliyyat  yerinə yetirilə bilmədi !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else MessageBox.Show("Əməliyyat uğurla yerinə yetirldi !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        catch (System.Data.SqlClient.SqlException)
                        {
                            MessageBox.Show("Satacağınız məhsulun sayı bazadakından artıqdır !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                AddToGridView();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Satacağınız məhsulu seçin!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            MessageBox.Show("Əməliyyat yerinə yetirilə bilmədi !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Əməliyyat uğurla yerinə yetirildi !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            textBox5.Clear();
                            textBox6.Clear();
                            textBox7.Clear();
                        }
                    }
                    AddToGridView();
                }
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Siləcəyiniz məhsulu seçin !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            Form frm = new TopSoldProducts();
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
        private void btn_ListOfDateForSale(object sender, EventArgs e)
        {
            Form frm = new ListOfProductsDueToDate(this);
            frm.Visible = true;
        }
        private void btn_click_search(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                dataGridView1.DataBindings.Clear();
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet ds = new DataSet();
                adapter.SelectCommand = new SqlCommand("uspFindProduct", con);
                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add("@name", SqlDbType.NVarChar).Value = textBox6.Text;
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
        }
    }
}

