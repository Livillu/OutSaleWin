using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WTools
{
    public partial class EditProduct : Form
    {
        DataRow DR;
        DataTable dt;
        public EditProduct(DataRow dr)
        {
            DR=dr;
            InitializeComponent();
        }

        public DataRow ResultProduct()
        {
            return DR;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string SupId = "";
            if (combbox1.SelectedIndex > -1) SupId = dt.Rows[combbox1.SelectedIndex][0].ToString();
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand($"UPDATE [Products] SET [MB051] ={textBox5.Text} ,[MB002]='{textBox2.Text}',[MB003]='{textBox3.Text}',[MB004]='{textBox4.Text}',[MB064] = {textBox6.Text},[SupId]='{SupId}',[CostPrice]={textBox7.Text} WHERE [MB001]='{textBox1.Text}'", conn1);
            cmd1.Connection.Open();
            if (DR == null)
            { 
                cmd1.CommandText = $"INSERT INTO Products([MB001],[MB002],[MB003],[MB004],[MB051],[MB064],[SupId],[CostPrice]) VALUES('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}',{textBox5.Text},{textBox6.Text},'{SupId}',{textBox7.Text})";                
            }
            if (cmd1.ExecuteNonQuery() > 0)
            {
                DR[0]= textBox1.Text;
                DR[1]= textBox2.Text;
                DR[2] = textBox3.Text;
                DR[5] = textBox4.Text;
                DR[3] = String.Format("{0:.00}", textBox5.Value);
                DR[4] = textBox6.Text.Trim();
                DR[7] = combbox1.Text.Trim();
                if (DR == null)
                {
                    if (textBox6.Value > 0)
                    {
                        cmd1.CommandText = $"INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut]) VALUES('{MainForm.UserId}','{textBox1.Text}',{textBox6.Text},1)";
                        cmd1.ExecuteNonQuery();
                    }

                }
                else
                {
                    decimal tmp=textBox6.Value- Convert.ToDecimal(DR[4]);
                    if(tmp > 0)
                    {
                        cmd1.CommandText = $"INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut]) VALUES('{MainForm.UserId}','{textBox1.Text}',{tmp},1)";
                        cmd1.ExecuteNonQuery();
                    }
                    else if (tmp < 0)
                    {
                        cmd1.CommandText = $"INSERT INTO [PDList]([userid],[MB001],[Quty],[InOut]) VALUES('{MainForm.UserId}','{textBox1.Text}',{Math.Abs(tmp)},-1)";
                        cmd1.ExecuteNonQuery();
                    }
                }
            }
        }

        private void EditProduct_Load(object sender, EventArgs e)
        {
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand("SELECT [SupId],[SupName] FROM [Support]", conn1);
            cmd1.Connection.Open();
            SqlDataReader sdr= cmd1.ExecuteReader();
            dt = new DataTable();
            dt.Load(sdr);
            combbox1.Items.Clear();
            combbox1.DataSource = dt;
            combbox1.DisplayMember = "SupName";
            if (DR != null)
            {
                textBox1.Text= DR[0].ToString();
                textBox1.ReadOnly= true;
                textBox2.Text= DR[1].ToString();
                textBox3.Text= DR[2].ToString();
                textBox4.Text= DR[5].ToString();
                textBox5.Text= DR[3].ToString();
                textBox6.Text= DR[4].ToString();
                combbox1.Text = DR[8].ToString();
                combbox1.Enabled = false;
            }
            else
            {
                textBox1.ReadOnly = false;
                textBox2.ReadOnly = false;
                combbox1.Enabled = true;
            }
        }
    }
}
