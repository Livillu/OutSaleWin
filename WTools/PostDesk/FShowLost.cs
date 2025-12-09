using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WTools.PostDesk
{
    public partial class FShowLost : Form
    {
        DataTable dt;
        public FShowLost(DataTable dt)
        {
            InitializeComponent();
            this.dt = dt;
        }

        private void FShowLost_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
        }
    }
}
