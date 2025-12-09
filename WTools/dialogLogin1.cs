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
    public partial class dialogLogin1 : Form
    {
        string msg;
        public dialogLogin1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn1 = new SqlConnection(MainForm.OutPoscon);
            SqlCommand cmd1 = new SqlCommand($"SELECT [usId] FROM [UsLogin] where [usId] = '{textBox1.Text}' and [usPw] = '{textBox2.Text}'", conn1);
            cmd1.Connection.Open();
            var tmp=cmd1.ExecuteScalar();
            if (tmp != null)
            {
                msg = tmp.ToString();
            }
        }
        public string Getmsg()
        {
            return msg;
        }
    }
}
