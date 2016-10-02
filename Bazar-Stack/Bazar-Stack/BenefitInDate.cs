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
    public partial class BenefitInDate : Form
    {
        const string constr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BazarStock;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public BenefitInDate()
        {
            InitializeComponent();
        }

        private void Get_Benefit_InDate(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspGetBenefitInDate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date1", SqlDbType.Date).Value = dateTimePicker1.Text;
                    cmd.Parameters.Add("@date2", SqlDbType.Date).Value = dateTimePicker2.Text;

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Benefit> listOfBenefit = new List<Benefit>();
                    while (reader.Read())
                    {
                        listOfBenefit.Add(new Benefit { Price = reader.GetDecimal(0), PriceOfProduct = reader.GetDecimal(1), CountOfSold = reader.GetInt32(2) });
                    }
                    if (listOfBenefit.Count == 0)
                    {
                        MessageBox.Show("Seçdiyiniz tarixdə məhsul əlavə olunmayıb !");
                    }
                    else
                    {

                        decimal sum = 0;
                        foreach (Benefit i in listOfBenefit)
                        {
                            decimal benefit;
                            benefit = (i.Price - i.PriceOfProduct) * i.CountOfSold;
                            sum = benefit + sum;
                        }
                        MessageBox.Show("Seçdiyiniz tarixdə əlavə olunan mallardan qazancıvız " + sum + "-bu qədərdir.");
                    }
                }
            }
        }
        class Benefit
        {
            public decimal Price { get; set; }
            public decimal PriceOfProduct { get; set; }
            public int CountOfSold { get; set; }
        }

        private void Get_All_Benefit(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspGetBenefit", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Benefit> listOfBenefit = new List<Benefit>();
                    while (reader.Read())
                    {
                        listOfBenefit.Add(new Benefit { Price = reader.GetDecimal(0), PriceOfProduct = reader.GetDecimal(1), CountOfSold = reader.GetInt32(2) });
                    }
                    decimal sum = 0;
                    foreach (Benefit i in listOfBenefit)
                    {
                        decimal benefit;
                        benefit = (i.PriceOfProduct - i.Price) * i.CountOfSold;
                        sum = benefit + sum;
                    }
                    MessageBox.Show("Ümumi qazancıvız " + sum + "-dır.");
                }
            }
        }

        private void Benefit_DueTo_SoldofDate(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("uspBenefitsDueToDay", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date1", SqlDbType.Date).Value = dateTimePicker1.Text;
                    cmd.Parameters.Add("@date2", SqlDbType.Date).Value = dateTimePicker2.Text;

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Benefit> listOfBenefit = new List<Benefit>();
                    while (reader.Read())
                    {
                        listOfBenefit.Add(new Benefit { Price = reader.GetDecimal(0), PriceOfProduct = reader.GetDecimal(1), CountOfSold = reader.GetInt32(2) });
                    }
                    if (listOfBenefit.Count == 0)
                    {
                        MessageBox.Show("Seçdiyiniz tarixdə məhsul satılmayıb olunmayıb !");
                    }
                    else
                    {

                        decimal sum = 0;
                        foreach (Benefit i in listOfBenefit)
                        {
                            decimal benefit;
                            benefit = (i.PriceOfProduct - i.Price) * i.CountOfSold;
                            sum = benefit + sum;
                        }
                        MessageBox.Show("Seçdiyiniz tarixdə əlavə olunan mallardan qazancıvız " + sum + "-bu qədərdir.");

                    }
                }
            }
        }
    }
}
