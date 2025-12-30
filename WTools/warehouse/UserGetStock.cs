using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WTools.warehouse
{
    public partial class UserGetStock : UserControl
    {
        DataTable PtDt;
        string Sno1;
        int UpdateQuty = 0;
        public UserGetStock()
        {
            InitializeComponent();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void UserGetStock_Load(object sender, EventArgs e)
        {
            tabControl1.TabPages.Clear();
            tabControl1.TabPages.Add(tabPage1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ButtonChange(0);
            Change(1);
            PtDt =new DataTable();
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand($"SELECT [Sno],[userid] ,a.[MB001], [MB002],[MB003],[MB004],[Quty], case [InOut] when 1 then '退料' else '領料' end [InOut],[Cdate] FROM [GetBackPt] a inner join [Products] b on a.MB001=b.MB001 ", conn1);
            cmd1.Connection.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();
            PtDt.Load(sdr);
            dataGridView1.DataSource = PtDt;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txmb001.Enabled = true;
            tabPage2.Text = "新增";
            ButtonChange(1);
            Change(2);
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow dgr = dataGridView1.SelectedRows[0];
                Sno1= dgr.Cells["Sno"].Value.ToString();
                UpdateQuty = Convert.ToInt32(dgr.Cells["Quty"].Value);
                numericUpDown1.Value = Convert.ToDecimal(UpdateQuty);
                txmb001.Text = dgr.Cells["MB001"].Value.ToString().Trim();
                lbProductName.Text = dgr.Cells["MB002"].Value.ToString().Trim();
                lbM004.Text = dgr.Cells["MB004"].Value.ToString().Trim();
                lbM003.Text = dgr.Cells["MB003"].Value.ToString().Trim();
                txmb001.Enabled = false;
                tabPage2.Text = "編輯";
                Change(2);
                ButtonChange(2);
            }
        }

        private void txmb001_Leave(object sender, EventArgs e)
        {
            if (txmb001.Text.Trim().Length > 3)
            {
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand($"SELECT [MB001], [MB002],[MB003],[MB004] FROM [Products] where [MB001]='{txmb001.Text.Trim()}' ", conn1);
                cmd1.Connection.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();
                if (sdr.Read())
                {
                    lbProductName.Text = sdr["MB002"].ToString().Trim();
                    lbM004.Text = sdr["MB004"].ToString().Trim();
                    lbM003.Text = sdr["MB003"].ToString().Trim();
                }
                cmd1.Connection.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Sno1 = "";
            UpdateQuty = 0;
            ButtonChange(0);
            Change(1);
        }

        private void ButtonChange(int index)
        {
            button2.Enabled = true;
            button6.Enabled = true;
            switch (index) {
                case 1://新增狀態
                    button2.Enabled =false;
                    break;
                case 2://編輯狀態
                    button6.Enabled =false;
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txmb001.Text.Trim().Length > 3)
            {
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand("", conn1);
                cmd1.Connection.Open();
                SqlTransaction sqlTransaction = null;
                sqlTransaction = conn1.BeginTransaction();
                cmd1.Transaction = sqlTransaction;
               
                string InOut = radioButton1.Checked ? "-1":"1";
                string UserType = radioButton1.Checked ? "2" : "3";
                string subType= radioButton1.Checked ? "-" : "+";
                if (txmb001.Enabled)
                {
                    cmd1.CommandText = $"INSERT INTO [GetBackPt]([userid],[MB001],[Quty],[InOut],[UserType],[Memo]) VALUES('{MainForm.UserId}','{txmb001.Text}',{numericUpDown1.Value},{InOut},'{UserType}','{textBox1.Text}')";
                    cmd1.ExecuteNonQuery();
                    cmd1.CommandText = $"UPDATE [Products] SET [MB064] = [MB064]{subType}{numericUpDown1.Value},UDate=getdate() WHERE [MB001]='{txmb001.Text.Trim()}'";
                    cmd1.ExecuteNonQuery();
                    cmd1.CommandText = $"INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut],[UserType]) VALUES('{MainForm.UserId}','{txmb001.Text.Trim()}',{numericUpDown1.Value},{InOut},'{UserType}')";
                    cmd1.ExecuteNonQuery();
                    cmd1.CommandText = $"INSERT INTO [PtLocation] ([Upid],[Itemid],[MB001],[Quty],[DateNumber]) VALUES('{MainForm.UserId}','{txmb001.Text.Trim()}',{numericUpDown1.Value},{InOut},'{UserType}')";
                    cmd1.ExecuteNonQuery();
                }
                else
                {
                    cmd1.CommandText = $"Update [GetBackPt] SET [userid]='{MainForm.UserId}',[MB001]='{txmb001.Text}',[Quty]={numericUpDown1.Value},[InOut]={InOut},[UserType]='{UserType}',[Memo]='{textBox1.Text}' WHERE Sno='{Sno1}')";
                    cmd1.ExecuteNonQuery();
                    int newQuty =Convert.ToInt32(numericUpDown1.Value) - UpdateQuty;
                    if (newQuty != 0)
                    {
                        cmd1.CommandText = $"UPDATE [Products] SET [MB064] = [MB064]{subType}({newQuty}),UDate=getdate() WHERE [MB001]='{txmb001.Text.Trim()}'";
                        cmd1.ExecuteNonQuery();
                        cmd1.CommandText = $"INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut],[UserType]) VALUES('{MainForm.UserId}','{txmb001.Text.Trim()}',{newQuty},{InOut},'{UserType}')";
                        cmd1.ExecuteNonQuery();
                        cmd1.CommandText = $" INSERT INTO [PtLocation] ([Upid],[Itemid],[MB001],[Quty]) VALUES('{MainForm.UserId}','{txmb001.Text.Trim()}',{newQuty},{InOut},'{UserType}')";
                        cmd1.ExecuteNonQuery();
                    }
                }
                try
                {
                    sqlTransaction.Commit();
                    button5.PerformClick();
                    MessageBox.Show("存檔完成....");
                }
                catch
                {
                    sqlTransaction.Rollback();
                    MessageBox.Show("存檔失敗!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
