namespace WTools.PostDesk
{
    partial class FShowLost
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MB001 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MB002 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MB064 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MB051 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MB001,
            this.MB002,
            this.MB064,
            this.MB051});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(32, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 31;
            this.dataGridView1.Size = new System.Drawing.Size(996, 494);
            this.dataGridView1.TabIndex = 0;
            // 
            // MB001
            // 
            this.MB001.DataPropertyName = "MB001";
            this.MB001.HeaderText = "品項";
            this.MB001.MinimumWidth = 8;
            this.MB001.Name = "MB001";
            this.MB001.ReadOnly = true;
            // 
            // MB002
            // 
            this.MB002.DataPropertyName = "MB002";
            this.MB002.HeaderText = "專案名稱";
            this.MB002.MinimumWidth = 8;
            this.MB002.Name = "MB002";
            this.MB002.ReadOnly = true;
            // 
            // MB064
            // 
            this.MB064.DataPropertyName = "MB064";
            this.MB064.HeaderText = "續買量";
            this.MB064.MinimumWidth = 8;
            this.MB064.Name = "MB064";
            this.MB064.ReadOnly = true;
            // 
            // MB051
            // 
            this.MB051.DataPropertyName = "MB051";
            this.MB051.HeaderText = "優惠";
            this.MB051.MinimumWidth = 8;
            this.MB051.Name = "MB051";
            this.MB051.ReadOnly = true;
            // 
            // FShowLost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 585);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FShowLost";
            this.Text = "折扣檢查";
            this.Load += new System.EventHandler(this.FShowLost_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn MB001;
        private System.Windows.Forms.DataGridViewTextBoxColumn MB002;
        private System.Windows.Forms.DataGridViewTextBoxColumn MB064;
        private System.Windows.Forms.DataGridViewTextBoxColumn MB051;
    }
}