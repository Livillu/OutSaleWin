using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WTools
{
    public partial class UserDiscount : UserControl
    {
        public DataTable dt;
        string buttonStatus = "";
        public UserDiscount()
        {
            InitializeComponent();
        }
        private void Change(int index)
        {
            tabControl1.TabPages.Clear();
            if (index == 2)
            {
                tabControl1.TabPages.Add(tabPage2);
            }
            else
            {
                tabControl1.TabPages.Add(tabPage1);
            }
        }
        private void ButtonChange(int index)
        {
            button2.Enabled = true;
            button6.Enabled = true;
            switch (index)
            {
                case 1://新增狀態
                    button2.Enabled = false;
                    button6.Enabled = false;
                    break;
            }
        }

        private void UserDiscount_Load(object sender, EventArgs e)
        {
            Change(1);
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

            if (numericUpDown1.Value > 0 && numericUpDown2.Value>0 && textBox1.Text != "" && textBox3.Text != "")
            {
                string sql = $"if(SELECT count(*) FROM [DiscountRule] where [GpSno]='{textBox1.Text}')=0 ";
                sql += "INSERT INTO [DiscountRule]([GpSno],[Quty],[Price],[SubMoney],[SubDiscount],[StDate],[EdDate],[GpName]) VALUES";
                sql += $"('{textBox1.Text}',{Quty},{Price},{SubMoney},{SubDiscount},'{dateTimePicker1.Value.ToString("yyyy-MM-dd")}','{dateTimePicker2.Value.ToString("yyyy-MM-dd")}','{textBox3.Text}') else ";
                sql += $"UPDATE [DiscountRule] SET [Quty] = {Quty},[Price] = {Price},[SubMoney] = {SubMoney},[SubDiscount] = {SubDiscount},[StDate] = '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}',[EdDate] = '{dateTimePicker2.Value.ToString("yyyy-MM-dd")}',[GpName] = '{textBox3.Text}' WHERE [GpSno]='{textBox1.Text}'";
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand(sql, conn1);
                cmd1.Connection.Open();
                if (cmd1.ExecuteNonQuery() > 0)
                {
                    UserDiscount_Load(null,null);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ButtonChange(0);
            Change(1);
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                buttonStatus = "Edit";
                DataGridViewRow dgr = dataGridView1.SelectedRows[0];
                tabPage2.Text = "編輯";
                Change(2);
                ButtonChange(1);
                
               // [GpSno],[Quty],[Price],[SubMoney],[SubDiscount],[StDate],[EdDate],[GpName]
                if (Convert.ToInt16(Convert.ToDecimal(dgr.Cells["Quty"].Value)) > 0 && Convert.ToDecimal(dgr.Cells["Price"].Value) == 0)
                {
                    radioButton3.Checked = true;
                    numericUpDown1.Value = Convert.ToInt16(dgr.Cells["Quty"].Value);
                }
                else if (Convert.ToInt16(dgr.Cells["Quty"].Value) == 0 && Convert.ToInt16(dgr.Cells["Price"].Value) > 0)
                {
                    radioButton4.Checked = true;
                    numericUpDown1.Value = Convert.ToInt16(dgr.Cells["Price"].Value);
                }
                else
                {
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    numericUpDown1.Value = 0;
                }

                if (Convert.ToInt16(dgr.Cells["SubMoney"].Value) > 0 && Convert.ToInt16(dgr.Cells["SubDiscount"].Value) == 0)
                {
                    radioButton1.Checked = true;
                    numericUpDown2.Value = Convert.ToInt16(dgr.Cells["SubMoney"].Value);
                }
                else if (Convert.ToInt16(dgr.Cells["SubMoney"].Value) == 0 && Convert.ToInt16(dgr.Cells["SubDiscount"].Value) > 0)
                {
                    radioButton2.Checked = true;
                    numericUpDown2.Value = Convert.ToInt16(dgr.Cells["SubDiscount"].Value);
                }
                else
                {
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    numericUpDown2.Value = 0;
                }

                dateTimePicker1.Value = Convert.ToDateTime(dgr.Cells["StDate"].Value);
                dateTimePicker2.Value = Convert.ToDateTime(dgr.Cells["EdDate"].Value);
                textBox3.Text = dgr.Cells["GpName"].Value.ToString();
                textBox1.Text = dgr.Cells["GpSno"].Value.ToString();
                textBox1.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buttonStatus = "New";
            radioButton1.Checked = true;
            radioButton3.Checked = true;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            textBox1.Enabled = true;
            textBox3.Text = "";
            tabPage2.Text = "新增";
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;
            ButtonChange(1);
            Change(2);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ButtonChange(0);
            Change(1);
        }
    }
}
