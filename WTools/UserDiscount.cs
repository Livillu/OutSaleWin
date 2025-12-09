using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WTools
{
    public partial class UserDiscount : UserControl
    {
        public DataTable dt;
        public UserDiscount()
        {
            InitializeComponent();
        }

        private void UserDiscount_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("SELECT distinct [GpSno] FROM [Products] where len(GpSno)>1 order by GpSno", conn1);
            cmd1.Connection.Open();
            cmd1.CommandText = "SELECT [GpSno],[Quty],[Price],[SubMoney],[SubDiscount],[StDate],[EdDate],[GpName] FROM [DiscountRule]";
            SqlDataReader dr1 = cmd1.ExecuteReader();
            dt.Load(dr1);
            dr1.Close();
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow dr = dt.Rows[e.RowIndex];
            if (dr != null)
            {
                if(Convert.ToInt16(dr["Quty"])>0 && Convert.ToInt16(dr["Price"]) == 0)
                {
                    radioButton3.Checked = true;
                    numericUpDown1.Value = Convert.ToInt16(dr["Quty"]);
                } else if (Convert.ToInt16(dr["Quty"]) == 0 && Convert.ToInt16(dr["Price"]) > 0)
                {
                    radioButton4.Checked = true;
                    numericUpDown1.Value = Convert.ToInt16(dr["Price"]);
                }
                else
                {
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    numericUpDown1.Value = 0;
                }

                if (Convert.ToInt16(dr["SubMoney"]) > 0 && Convert.ToInt16(dr["SubDiscount"]) == 0)
                {
                    radioButton1.Checked = true;
                    numericUpDown2.Value = Convert.ToInt16(dr["SubMoney"]);
                }
                else if (Convert.ToInt16(dr["SubMoney"]) == 0 && Convert.ToInt16(dr["SubDiscount"]) > 0)
                {
                    radioButton2.Checked = true;
                    numericUpDown2.Value = Convert.ToInt16(dr["SubDiscount"]);
                }
                else
                {
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    numericUpDown2.Value = 0;
                }

                dateTimePicker1.Value = Convert.ToDateTime(dr["StDate"]);
                dateTimePicker2.Value = Convert.ToDateTime(dr["EdDate"]);
                textBox3.Text= dr["GpName"].ToString();
                comboBox1.Text = dr["GpSno"].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal Quty=0, Price=0, SubMoney=0, SubDiscount=0;
            if (radioButton3.Checked)
            {
                Quty=numericUpDown1.Value;
            }
            else if (radioButton4.Checked)
            {
                Price = numericUpDown1.Value;
            }

            if (radioButton1.Checked)
            {
                SubMoney = numericUpDown2.Value;
            }
            else if (radioButton2.Checked)
            {
                SubDiscount = numericUpDown2.Value;
            }

            if (numericUpDown1.Value > 0 && numericUpDown2.Value>0 && comboBox1.Text != "" && textBox3.Text != "")
            {
                string sql = $"if(SELECT count(*) FROM [DiscountRule] where [GpSno]='{comboBox1.Text}')=0 ";
                sql += "INSERT INTO [DiscountRule]([GpSno],[Quty],[Price],[SubMoney],[SubDiscount],[StDate],[EdDate],[GpName]) VALUES";
                sql += $"('{comboBox1.Text}',{Quty},{Price},{SubMoney},{SubDiscount},'{dateTimePicker1.Value.ToString("yyyy-MM-dd")}','{dateTimePicker2.Value.ToString("yyyy-MM-dd")}','{textBox3.Text}') else ";
                sql += $"UPDATE [DiscountRule] SET [Quty] = {Quty},[Price] = {Price},[SubMoney] = {SubMoney},[SubDiscount] = {SubDiscount},[StDate] = '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}',[EdDate] = '{dateTimePicker2.Value.ToString("yyyy-MM-dd")}',[GpName] = '{textBox3.Text}' WHERE [GpSno]='{comboBox1.Text}'";
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand(sql, conn1);
                cmd1.Connection.Open();
                if (cmd1.ExecuteNonQuery() > 0)
                {
                    UserDiscount_Load(null,null);
                }
            }
        }
    }
}
