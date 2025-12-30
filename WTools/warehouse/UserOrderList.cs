using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WTools.warehouse
{
    public partial class UserOrderList : UserControl
    {
        DataTable DSupport; //datagrid
        DataTable Sdt; //庫別
        DataTable Sdt1;//儲位別
        DataTable Sdt2;//選擇儲位
        public UserOrderList()
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
            button6.Enabled = true;
            button7.Enabled = true;
            switch (index)
            {
                case 1://新增狀態
                    button6.Enabled = false;
                    button7.Enabled = false;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand($"SELECT a.[SupId],SupName,[Od_No],[TotalPrice],[Cdate],[UsId] FROM [OrderProductM] a join Support d on a.SupId=d.SupId where [Od_No]='{textBox2.Text}'", conn1);
            cmd1.Connection.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();
            DataTable tmpdt = new DataTable();
            tmpdt.Load(sdr);
            dataGridView2.DataSource = tmpdt;
            if (tmpdt.Rows.Count > 0)
            {
                textBox3.Text = tmpdt.Rows[0]["Od_No"].ToString();
                label1.Text = tmpdt.Rows[0]["SupId"].ToString();
                label5.Text = tmpdt.Rows[0]["SupName"].ToString();
            }
            else
            {
                textBox3.Text = "";
                label1.Text = "";
                label5.Text = "";
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex > -1)
            {
                comboBox3.Items.Clear();
                comboBox3.Text = "";
                Sdt2 = new DataTable();
                Sdt2 = Sdt1.Clone();
                foreach (DataRow dataRow in Sdt1.Rows)
                {
                    string s1 = dataRow["Upitem"].ToString();
                    string s2 = Sdt.Rows[comboBox2.SelectedIndex]["sno"].ToString();
                    if (s1 == s2)
                    {
                        Sdt2.ImportRow(dataRow);
                        comboBox3.Items.Add(dataRow["Name"].ToString());
                    }
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                string quty = DSupport.Rows[e.RowIndex][e.ColumnIndex].ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DSupport.Rows.Count > 0)
            {
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand("", conn1);
                cmd1.Connection.Open();
                SqlTransaction sqlTransaction = null;
                sqlTransaction = conn1.BeginTransaction();
                cmd1.Transaction = sqlTransaction;
                foreach (DataRow dr in DSupport.Rows)
                {
                    var tmp = Sdt2.Rows[comboBox3.SelectedIndex];
                    string sql = "INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut]) VALUES";
                    sql += $"('{MainForm.UserId}','{dr["MB001"].ToString()}',{dr["Quty1"].ToString()},1)";
                    cmd1.CommandText = sql;
                    cmd1.ExecuteNonQuery();
                    sql = "INSERT INTO [PtLocation] ([Upid],[Itemid],[MB001],[Quty],[DateNumber]) VALUES";
                    sql += $"({Sdt2.Rows[comboBox3.SelectedIndex]["Upitem"]},{Sdt2.Rows[comboBox3.SelectedIndex]["sno"]},'{dr["MB001"].ToString()}',{dr["Quty1"].ToString()},{DateTime.Today.ToString("yyyyMMdd")+"001"})";
                    cmd1.CommandText = sql;
                    cmd1.ExecuteNonQuery();
                    sql = $"UPDATE [Products] SET [MB064] = [MB064]+{dr["Quty1"].ToString()},UDate=getdate() WHERE [MB001]='{dr["MB001"].ToString()}'";
                    cmd1.CommandText = sql;
                    cmd1.ExecuteNonQuery();                   
                }
                try
                {
                    sqlTransaction.Commit();
                    MessageBox.Show("存檔完成....");
                }
                catch
                {
                    sqlTransaction.Rollback();
                    MessageBox.Show("存檔失敗!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UserOrderList_Load(object sender, EventArgs e)
        {
            DSupport = new DataTable();

            DataColumn dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "Od_No";
            dataColumn.AllowDBNull = false;
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "SupId";
            dataColumn.AllowDBNull = false;
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "SupName";
            dataColumn.AllowDBNull = false;
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "MB001";
            dataColumn.AllowDBNull = false;
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "MB002";
            dataColumn.AllowDBNull = false;
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "MB003";
            dataColumn.AllowDBNull = false;
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(string);
            dataColumn.ColumnName = "MB004";
            DSupport.Columns.Add(dataColumn);

            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(decimal);
            dataColumn.ColumnName = "Quty";
            DSupport.Columns.Add(dataColumn);
            dataColumn = new DataColumn();
            dataColumn.AllowDBNull = false;
            dataColumn.DataType = typeof(decimal);
            dataColumn.ColumnName = "Quty1";
            DSupport.Columns.Add(dataColumn);
            bindingSource1.DataSource = DSupport;
            dataGridView1.DataSource = bindingSource1;

            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("SELECT [sno],[Name] FROM [Stockhouse] where [Upitem]=0 order by[sno]", conn1);
            cmd1.Connection.Open();
            SqlDataReader sdr = cmd1.ExecuteReader();
            Sdt = new DataTable();
            Sdt.Load(sdr);
            sdr.Close();
            foreach (DataRow dr in Sdt.Rows)
            {
                comboBox2.Items.Add(dr["Name"].ToString());
                comboBox1.Items.Add(dr["Name"].ToString());
            }
            cmd1 = new SqlCommand("SELECT [sno],[Name],[Upitem] FROM [Stockhouse] where [Upitem]>0 order by[sno]", conn1);
            SqlDataReader sdr1 = cmd1.ExecuteReader();
            Sdt1 = new DataTable();
            Sdt1.Load(sdr1);
            sdr1.Close();
            Change(1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                comboBox4.Items.Clear();
                comboBox4.Text = "";
                Sdt2 = new DataTable();
                Sdt2 = Sdt1.Clone();
                foreach (DataRow dataRow in Sdt1.Rows)
                {
                    string s1 = dataRow["Upitem"].ToString();
                    string s2 = Sdt.Rows[comboBox1.SelectedIndex]["sno"].ToString();
                    if (s1 == s2)
                    {
                        Sdt2.ImportRow(dataRow);
                        comboBox4.Items.Add(dataRow["Name"].ToString());
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() !="" && lbProductName.Text !="")
            {
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand("", conn1);
                cmd1.Connection.Open();
                SqlTransaction sqlTransaction = null;
                sqlTransaction = conn1.BeginTransaction();
                cmd1.Transaction = sqlTransaction;
                var tmp = Sdt2.Rows[comboBox4.SelectedIndex];
                string sql = "INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut]) VALUES";
                sql += $"('{MainForm.UserId}','{textBox1.Text.Trim()}',{numericUpDown1.Value},1)";
                cmd1.CommandText = sql;
                cmd1.ExecuteNonQuery();
                sql = "INSERT INTO [PtLocation] ([Upid],[Itemid],[MB001],[Quty],[DateNumber]) VALUES";
                sql += $"({Sdt2.Rows[comboBox4.SelectedIndex]["Upitem"]},{Sdt2.Rows[comboBox4.SelectedIndex]["sno"]},'{textBox1.Text.Trim()}',{numericUpDown1.Value},{DateTime.Today.ToString("yyyyMMdd") + "001"})";
                cmd1.CommandText = sql;
                cmd1.ExecuteNonQuery();
                sql = $"UPDATE [Products] SET [MB064] = [MB064]+{numericUpDown1.Value},UDate=getdate() WHERE [MB001]='{textBox1.Text.Trim()}'";
                cmd1.CommandText = sql;
                cmd1.ExecuteNonQuery();
                
                try
                {
                    sqlTransaction.Commit();
                    MessageBox.Show("存檔完成....");
                }
                catch
                {
                    sqlTransaction.Rollback();
                    MessageBox.Show("存檔失敗!!!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand($"SELECT [MB001],[MB002],[MB003],[MB004] FROM [Products] where MB001='{textBox1.Text.Trim()}'", conn1);
                cmd1.Connection.Open();
                SqlDataReader sdr1 = cmd1.ExecuteReader();
                if (sdr1.Read())
                {
                    lbProductName.Text = sdr1["MB002"].ToString();
                    lbM003.Text = sdr1["MB003"].ToString();
                    lbM004.Text = sdr1["MB004"].ToString();
                }
                else
                {
                    lbProductName.Text = "";
                    lbM003.Text = "";
                    lbM004.Text = "";
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            lbM003.Text = "";
            lbM004.Text = "";
            lbProductName.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            textBox1.Text = "";
            numericUpDown1.Value = 0;
            tabPage2.Text = "手動入庫";
            ButtonChange(1);
            Change(2);
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow dgv= dataGridView2.SelectedRows[0];
                SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
                SqlCommand cmd1 = new SqlCommand($"SELECT a.[Od_No],d.SupId,SupName,[MB001],[MB002],[MB003],[MB004],[Quty],[Quty] [Quty1] FROM [OrderProuductT] a inner join OrderProductM b on a.[Od_No]=b.Od_No inner join Products c on a.[PtNo]=c.MB001 inner join Support d on b.SupId=d.SupId where a.[Od_No]='{dgv.Cells["Od_No"].Value.ToString()}'", conn1);
                cmd1.Connection.Open();
                SqlDataReader sdr = cmd1.ExecuteReader();
                DSupport = new DataTable();
                DSupport.Load(sdr);
                dataGridView1.DataSource = DSupport;
                if (DSupport.Rows.Count > 0)
                {
                    textBox3.Text = DSupport.Rows[0]["Od_No"].ToString();
                    label1.Text = DSupport.Rows[0]["SupId"].ToString();
                    label5.Text = DSupport.Rows[0]["SupName"].ToString();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ButtonChange(0);
            Change(1);
        }
    }
}
