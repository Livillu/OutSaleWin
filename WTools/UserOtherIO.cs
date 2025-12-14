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
    public partial class UserOtherIO : UserControl
    {
        DataTable TTD;
        int tbInOut;
        public UserOtherIO()
        {
            InitializeComponent();
        }
        private void prgstatus(int index)
        {
            tbMark.Enabled = false;
            tbSno.Enabled = false;
            tbMark.Enabled = false;
            tbMB001.Enabled = false;
            tbQuty.Enabled = false;
            tbPrice.Enabled = false;
            if (index == 0)
            {
                tbMark.Enabled = true;
                tbSno.Enabled = true;
                tbMark.Enabled = true;
                tbMB001.Enabled = true;
                tbQuty.Enabled = true;
                tbPrice.Enabled = true;
            }
            if (index == 1)
            {
                tbMark.Enabled = true;
                tbMark.Enabled = true;
                tbMB001.Enabled = true;
                tbQuty.Enabled = true;
                tbPrice.Enabled = true;
            }
        }

        private void Setbutton(string tp)
        {
            if (tp == "A")
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button5.Enabled = true;
                button4.Enabled = true;
            }
            if (tp == "S")
            {
                prgstatus(5);
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = false;
                button4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TTD = new DataTable();
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("", conn1);
            cmd1.Connection.Open();
            string sql = "SELECT [Sno],[MB001],[Quty],[Price],[InOut],[Cdate],[Mark],[Quty]*[Price]*[InOut] Total FROM [OtherCost] where 1=1";
            if (textBox10.Text.Trim() != "") sql += $" and MB001 like '%{textBox10.Text}%'";
            if (textBox11.Text.Trim() != "") sql += $" and Sno like '%{textBox11.Text}%'";
            cmd1.CommandText = sql;
            SqlDataReader sdr = cmd1.ExecuteReader();
            TTD.Load(sdr);
            dataGridView1.DataSource = TTD;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Setbutton("A");
            tbMark.Text = "";
            tbSno.Text = "";
            tbQuty.Text = "";
            tbPrice.Text = "";
            tbMB001.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            prgstatus(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tbSno.Text.Trim() != "")
            {
                Setbutton("A");
                prgstatus(1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Setbutton("S");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Setbutton("S");
            if (radioButton1.Checked) tbInOut = -1;
            else tbInOut = 1;
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand($"SELECT count(*) FROM [OtherCost] where [Sno]='{tbSno.Text.Trim()}'", conn1);
            cmd1.Connection.Open();
            string sql;
            if (Convert.ToInt16(cmd1.ExecuteScalar()) > 0)
            {
                sql = $"UPDATE [OtherCost] SET [MB001] ='{tbMB001.Text}' ,[Quty] ='{tbQuty.Value}' ,[Price] ='{tbPrice.Value}' ,[InOut] ='{tbInOut}' ,[Mark] ='{tbMark.Text}',[Cdate]='{dateTimePicker1.Value.ToString("yyyy-MM-dd hh:mm:ss")}' WHERE [Sno] = '{tbSno.Text}'";
                cmd1.CommandText = sql;
            }
            else
            {
                sql = $"INSERT INTO [OtherCost]([Sno],[MB001],[Quty],[Price],[InOut],[Mark],[Cdate]) VALUES('{tbSno.Text}','{tbMB001.Text}',{tbQuty.Value},{tbPrice.Value},{tbInOut},'{tbMark.Text}',[Cdate]='{dateTimePicker1.Value.ToString("yyyy-MM-dd hh:mm:ss")}')";
                cmd1.CommandText = sql;
            }
            cmd1.ExecuteNonQuery();
            prgstatus(2);
            button1.PerformClick();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataRow dr = TTD.NewRow();
            dr = TTD.Rows[e.RowIndex];
            tbSno.Text = dr["Sno"].ToString();
            tbMB001.Text = dr["MB001"].ToString();
            tbQuty.Text = dr["Quty"].ToString();
            tbPrice.Text = dr["Price"].ToString();
            tbMark.Text = dr["Mark"].ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dr["Cdate"]);
            if (Convert.ToInt16(dr["InOut"])>0 )radioButton2.Checked=true;
            else radioButton1.Checked = true;
        }
    }
}
