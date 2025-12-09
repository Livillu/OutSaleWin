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
using System.Xml.Linq;

namespace WTools
{
    public partial class UserAssember : UserControl
    {
        DataTable DT, DTsale, DT1;
        public UserAssember()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataRow udr = DT.Rows[e.RowIndex];
            textBox1.Text = udr["GpId"].ToString();
            textBox2.Text = udr["GpName"].ToString();
            numericUpDown1.Value = Convert.ToDecimal(udr["GpPrice"]);
            string sql = $"SELECT [MB001],(SELECT TOP (1) [MB002] FROM [Products] WHERE [MB001]=a.[MB001]) [MB002],[MB064] FROM [GpProuductT] a WHERE [GpId]='{udr["GpId"].ToString()}'";
            SqlConnection conn = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            DTsale.Rows.Clear();
            if (sdr.HasRows)
            {
                if (button1.Enabled == false)
                {
                    button1.Enabled = true;
                }
                while (sdr.Read())
                {
                    DataRow dr2 = DTsale.NewRow();
                    dr2[0] = sdr[0];
                    dr2[1] = sdr[1];
                    dr2[2] = sdr[2];
                    DTsale.Rows.Add(dr2);     
                }
         
            }
        }

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DTsale.Rows.RemoveAt(e.RowIndex);
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataRow dr = DTsale.Rows[e.RowIndex];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DTsale.Rows.Count > 0 && textBox1.Text != "" && textBox2.Text != "" && numericUpDown1.Value >0)
            {
                string sql = $"SELECT count(*) FROM [GpProductM] where [GpId]='{textBox1.Text}'";
                SqlConnection conn = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Connection.Open();
                if (Convert.ToInt16(cmd.ExecuteScalar()) == 0)
                {
                    sql = "INSERT INTO [GpProductM] ([GpId], [GpName], [GpPrice]) VALUES(";
                    sql += $"'{textBox1.Text}','{textBox2.Text}',{numericUpDown1.Value})";
                    cmd.CommandText = sql;
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        foreach (DataRow dr in DTsale.Rows)
                        {
                            sql = "INSERT INTO [GpProuductT] ([GpId],[MB001],[MB064]) VALUES(";
                            sql += $"'{textBox1.Text}','{dr[0]}',{dr[2]})";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        button4.PerformClick();
                    }
                }
                else
                {
                    sql = $"UPDATE [GpProductM] SET [GpName]='{textBox2.Text}',[GpPrice]={numericUpDown1.Value} where [GpId]='{textBox1.Text}'";
                    cmd.CommandText= sql;
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        sql = $"DELETE FROM [GpProuductT] where [GpId]='{textBox1.Text}'";
                        cmd.CommandText = sql;
                        cmd.ExecuteNonQuery();
                        foreach (DataRow dr in DTsale.Rows)
                        {
                            sql = "INSERT INTO [GpProuductT] ([GpId],[MB001],[MB064]) VALUES(";
                            sql += $"'{textBox1.Text}','{dr[0]}',{dr[2]})";
                            cmd.CommandText = sql;
                            cmd.ExecuteNonQuery();
                        }
                        button4.PerformClick();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sql = $"SELECT TOP(1) [MB001],[MB002] FROM [Products] WHERE [MB001]='{textBox4.Text}'";
            SqlConnection conn = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {    
                dialogform dl = new dialogform(sdr[1].ToString());
                dl.productName = sdr[1].ToString();
                dl.Text = "數量";
                DialogResult dr = dl.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    DataRow dr2 = DTsale.NewRow();
                    dr2[0] = sdr[0];
                    dr2[1] = sdr[1];
                    dr2[2] = dl.GetMsg();
                    DTsale.Rows.Add(dr2);
                    if (button1.Enabled == false)
                    {
                        button1.Enabled = true;
                    }
                    textBox4.Text = "";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DTsale.Rows.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            numericUpDown1.Value =0;
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DT = new DataTable();
            string sql = "SELECT [GpId],[GpName],[GpPrice] FROM [GpProductM] WHERE 1=1";
            SqlConnection conn = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.HasRows)
            {
                DT.Load(sdr);
                dataGridView1.DataSource = DT;
            }         
        }

        private void UserAssember_Load(object sender, EventArgs e)
        {
            DTsale = new DataTable();
            DataColumn dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "MB001";
            dataColumn.AllowDBNull = false;
            DTsale.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "MB002";
            DTsale.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(int);
            dataColumn.ColumnName = "MB064";
            dataColumn.ReadOnly = false;
            DTsale.Columns.Add(dataColumn);
            dataGridView2.DataSource = DTsale;            
        }
    }
}
