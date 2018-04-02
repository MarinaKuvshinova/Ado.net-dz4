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

namespace _21._03
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=HOME-ПК; Initial Catalog=Purchase; Integrated Security=true;";
        SqlConnection sqlConnection = null;
        SqlDataReader reader = null;
        SqlCommand sqlCommand = null;
        SqlDataAdapter sqlDateAdapter = null;
        DataSet dataSet = null;
        SqlCommandBuilder sqlCommanBuilder = null;
        //DataTable dataTable = null;
        public Form1()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(connectionString);
            btnBuy.Click += btnBuy_Click;
            update();
        }

        private void update()
        {
            try
            {
                sqlConnection.Open();
                string query = "select Count from Buyers";
                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    label1.Text = reader.GetValue(0).ToString();
                }
                reader.Close();
                query = "select Count from Sellers";
                SqlCommand cmd2 = new SqlCommand(query, sqlConnection);
                reader = cmd2.ExecuteReader();
                while (reader.Read())
                {
                    label2.Text = reader.GetValue(0).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                    sqlConnection.Close();
                if (reader != null)
                    reader.Close();
            }
        }

        private void btnBuy_Click(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(connectionString);
            sqlCommand = sqlConnection.CreateCommand();
            SqlTransaction sqlTransaction = null;
            try
            {
                sqlConnection.Open();
                sqlTransaction = sqlConnection.BeginTransaction();
                sqlCommand.Transaction = sqlTransaction;

                sqlCommand.CommandText = @"insert into Checks values (1,1,default);";
                sqlCommand.ExecuteNonQuery();

                sqlCommand.CommandText = @"UPDATE Buyers SET Count+=1 WHERE ID_Buyer=1;";
                sqlCommand.ExecuteNonQuery();

                if (Convert.ToInt32(label2.Text) - 1 < 0)
                {
                    throw new Exception("Нет больше что покупать");
                }
                else
                {
                    sqlCommand.CommandText = @"UPDATE Sellers SET Count-=1 WHERE ID_Seller=1;";
                    sqlCommand.ExecuteNonQuery();
                }

                sqlTransaction.Commit();
                dataGridView1.DataSource = null;
                dataSet = new DataSet();
                string query = @"select * from Checks";
                sqlDateAdapter = new SqlDataAdapter(query, sqlConnection);
                sqlCommanBuilder = new SqlCommandBuilder(sqlDateAdapter);
                sqlDateAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                sqlTransaction.Rollback();
            }
            finally
            {
                sqlConnection.Close();
                update();
            }
        }
    }
}
