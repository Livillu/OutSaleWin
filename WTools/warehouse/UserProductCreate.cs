using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WTools.warehouse
{
    public partial class UserProductCreate : UserControl
    {
        DataTable DT,dt;
        string buttonStatus;
        public UserProductCreate()
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

        private void button1_Click(object sender, EventArgs e)
        {
            ButtonChange(0);
            Change(1);
            string sqlparam = "";
            if (textBox2.Text != "") sqlparam += " WHERE MB001 LIKE '%" + textBox2.Text + "%' OR  MB002 LIKE '%" + textBox2.Text + "%'";
            string sqlstring = "SELECT [MB001],[MB002],[MB003],[MB051],[MB064],[MB004],[GpSno],[SupId],(SELECT TOP (1) [SupName] FROM [Support] where [SupId]=a.SupId) [SupName],[CostPrice] FROM [Products] a " + sqlparam;

            SqlConnection conn = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd = new SqlCommand(sqlstring, conn);
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            DT=new DataTable();
            DT.Load(sdr);
            dataGridView1.DataSource = DT;
            //label3.Text = DT.Rows.Count.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string SupId = "";
            if (combbox1.SelectedIndex > -1) SupId = dt.Rows[combbox1.SelectedIndex][0].ToString();
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("", conn1);
            cmd1.Connection.Open();
            if (textBox9.Text.ToString() != "")
            {
                if (buttonStatus == "Edit")
                {
                    cmd1.CommandText = $"UPDATE [Products] SET [MB051] ={textBox5.Text} ,[MB002]='{textBox6.Text}',[MB003]='{textBox3.Text}',[MB004]='{textBox4.Text}',[SupId]='{SupId}',[CostPrice]={textBox7.Text},[GpSno]='{textBox8.Text}' WHERE [MB001]='{textBox9.Text}'";
                }
                else if (buttonStatus == "New")
                {
                    cmd1.CommandText = $"INSERT INTO Products([MB001],[MB002],[MB003],[MB004],[MB051],[MB064],[SupId],[CostPrice],[GpSno]) VALUES('{textBox9.Text}','{textBox6.Text}','{textBox3.Text}','{textBox4.Text}',{textBox5.Text},0,'{SupId}',{textBox7.Text},'{textBox8.Text}')";
                }
                if(cmd1.ExecuteNonQuery()>0)
                {
                    button1.PerformClick();
                    MessageBox.Show("存檔完成....");
                }
                else
                {
                    MessageBox.Show("存檔失敗!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buttonStatus = "New";
            textBox5.Value = 0;
            textBox7.Value = 0;
            textBox9.Text = "";
            textBox6.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox8.Text = "";
            textBox9.Enabled = true;
            tabPage2.Text = "新增";
            ButtonChange(1);
            Change(2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                buttonStatus = "Edit";
                DataGridViewRow dgr = dataGridView1.SelectedRows[0];
               // "SELECT [MB001],[MB002],[MB003],[MB051],[MB064],[MB004],[GpSno],[SupId],(SELECT TOP (1) [SupName] FROM [Support] where [SupId]=a.SupId) [SupName],[CostPrice] FROM [Products]
                textBox5.Value = Convert.ToDecimal(dgr.Cells["MB051"].Value);
                textBox7.Value = Convert.ToDecimal(dgr.Cells["CostPrice"].Value);
                textBox9.Text = dgr.Cells["MB001"].Value.ToString().Trim();
                textBox6.Text = dgr.Cells["MB002"].Value.ToString().Trim();
                textBox3.Text = dgr.Cells["MB003"].Value.ToString().Trim();
                textBox4.Text = dgr.Cells["MB004"].Value.ToString().Trim();
                textBox8.Text = dgr.Cells["GpSno"].Value.ToString().Trim();
                textBox9.Enabled = false;
                tabPage2.Text = "編輯";
                Change(2);
                ButtonChange(1);
            }
        }

        private void UserProductCreate_Load(object sender, EventArgs e)
        {
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("SELECT [SupId],[SupName] FROM [Support]", conn1);
            cmd1.Connection.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            combbox1.Items.Clear();
            combbox1.DataSource = dt;
            combbox1.DisplayMember = "SupName";
            Change(1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ButtonChange(0);
            Change(1);
        }
    }
}
