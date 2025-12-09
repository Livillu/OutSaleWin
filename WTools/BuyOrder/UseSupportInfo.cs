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

namespace WTools.BuyOrder
{
    public partial class UseSupportInfo : UserControl
    {
        DataTable TTD;
        public UseSupportInfo()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TTD = new DataTable();
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("", conn1);
            cmd1.Connection.Open();
            string sql = "SELECT [SupId] ,[SupName] ,[SupCname] ,[SupTel] ,[SupAddr] ,[SupSno] ,[Boss] ,[CTel] ,[SupEmail] ,[SupWeb] FROM [Support] where 1=1";
            if (textBox10.Text.Trim() != "") sql += $" and SupName like '%{textBox10.Text}%'";
            if (textBox9.Text.Trim() != "") sql += $" and SupSno like '%{textBox9.Text}%'";
            if (textBox11.Text.Trim() != "") sql += $" and SupId like '%{textBox11.Text}%'";
            cmd1.CommandText = sql;
            SqlDataReader sdr = cmd1.ExecuteReader();
            TTD.Load(sdr);
            dataGridView1.DataSource = TTD;
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataRow dr = TTD.NewRow();
            dr = TTD.Rows[e.RowIndex];
            tbBoss.Text = dr["Boss"].ToString();
            tbCTel.Text = dr["CTel"].ToString();
            tbSupAddr.Text = dr["SupAddr"].ToString();
            tbSupCname.Text = dr["SupCname"].ToString();
            tbSupEmail.Text = dr["SupEmail"].ToString();
            tbSupId.Text = dr["SupId"].ToString();
            tbSupName.Text = dr["SupName"].ToString();
            tbSupSno.Text = dr["SupSno"].ToString();
            tbSupTel.Text = dr["SupTel"].ToString();
            tbSupWeb.Text = dr["SupWeb"].ToString();

        }

        private void prgstatus(int index)
        {
            tbBoss.Enabled = false;
            tbCTel.Enabled = false;
            tbSupAddr.Enabled = false;
            tbSupCname.Enabled = false;
            tbSupEmail.Enabled = false;
            tbSupId.Enabled = false;
            tbSupName.Enabled = false;
            tbSupSno.Enabled = false;
            tbSupTel.Enabled = false;
            tbSupWeb.Enabled = false;
            if (index == 0)
            {
                tbBoss.Enabled = true;
                tbCTel.Enabled = true;
                tbSupAddr.Enabled = true;
                tbSupCname.Enabled = true;
                tbSupEmail.Enabled = true;
                tbSupId.Enabled = true;
                tbSupName.Enabled = true;
                tbSupSno.Enabled = true;
                tbSupTel.Enabled = true;
                tbSupWeb.Enabled = true;
            }
            if (index == 1)
            {
                tbBoss.Enabled = true;
                tbCTel.Enabled = true;
                tbSupAddr.Enabled = true;
                tbSupCname.Enabled = true;
                tbSupEmail.Enabled = true;
                tbSupName.Enabled = true;
                tbSupSno.Enabled = true;
                tbSupTel.Enabled = true;
                tbSupWeb.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Setbutton("A");
            tbBoss.Text = "";
            tbCTel.Text = "";
            tbSupAddr.Text = "";
            tbSupCname.Text = "";
            tbSupEmail.Text = "";
            tbSupId.Text = "";
            tbSupName.Text = "";
            tbSupSno.Text = "";
            tbSupTel.Text = "";
            tbSupWeb.Text = "";
            prgstatus(0);
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
        private void button3_Click(object sender, EventArgs e)
        {
            if (tbSupId.Text.Trim() != "")
            {
                Setbutton("A");
                prgstatus(1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Setbutton("S");
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand($"SELECT count(*) FROM [Support] where [SupId]='{tbSupId.Text.Trim()}'", conn1);
            cmd1.Connection.Open();
            string sql;
            if (Convert.ToInt16(cmd1.ExecuteScalar()) > 0)
            {
                sql = $"UPDATE [Support] SET [SupName] ='{tbSupName.Text}' ,[SupCname] ='{tbSupCname.Text}' ,[SupTel] ='{tbSupTel.Text}' ,[SupAddr] ='{tbSupAddr.Text}' ,[SupSno] ='{tbSupSno.Text}' ,[Boss] ='{tbBoss.Text}' ,[CTel] ='{tbCTel.Text}' ,[SupEmail] ='{tbSupEmail.Text}' ,[SupWeb] ='{tbSupWeb.Text}' WHERE [SupId] = '{tbSupId.Text}'";
                cmd1.CommandText = sql;
            }
            else
            {
                sql = $"INSERT INTO [Support]([SupId],[SupName],[SupCname],[SupTel],[SupAddr],[SupSno],[Boss],[CTel],[SupEmail],[SupWeb]) VALUES('{tbSupId.Text}','{tbSupName.Text}','{tbSupCname.Text}','{tbSupTel.Text}','{tbSupAddr.Text}','{tbSupSno.Text}','{tbBoss.Text}','{tbCTel.Text}','{tbSupEmail.Text}','{tbSupWeb.Text}')";
                cmd1.CommandText = sql;
            }
            cmd1.ExecuteNonQuery();
            prgstatus(2);
            button1.PerformClick();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Setbutton("S");
        }

        private void UseSupportInfo_Load(object sender, EventArgs e)
        {
            prgstatus(2);
        }
    }
}
