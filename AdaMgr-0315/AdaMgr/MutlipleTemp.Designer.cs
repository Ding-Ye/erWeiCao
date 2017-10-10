namespace AdaMgr
{
    partial class MutlipleTemp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MutlipleTemp));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AddDatetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.t13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.dateTimePicker2);
            this.groupBox3.Controls.Add(this.dateTimePicker1);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.Color.Black;
            this.groupBox3.Location = new System.Drawing.Point(65, -3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(783, 41);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(76, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 19);
            this.label1.TabIndex = 4;
            this.label1.Text = "请选择查询时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(424, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "结束:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(245, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "起始:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(467, 13);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(103, 21);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(288, 12);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(103, 21);
            this.dateTimePicker1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(642, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 29);
            this.button2.TabIndex = 0;
            this.button2.Text = "查询";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AddDatetime,
            this.t1,
            this.t2,
            this.t3,
            this.t4,
            this.t5,
            this.t6,
            this.t7,
            this.t8,
            this.t9,
            this.t10,
            this.t11,
            this.t12,
            this.t13});
            this.dataGridView1.Location = new System.Drawing.Point(-41, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(948, 551);
            this.dataGridView1.TabIndex = 5;
            // 
            // AddDatetime
            // 
            this.AddDatetime.DataPropertyName = "AddDatetime";
            this.AddDatetime.HeaderText = "采样时间";
            this.AddDatetime.Name = "AddDatetime";
            this.AddDatetime.ReadOnly = true;
            this.AddDatetime.Width = 102;
            // 
            // t1
            // 
            this.t1.DataPropertyName = "t1";
            this.t1.HeaderText = "温度1";
            this.t1.Name = "t1";
            this.t1.ReadOnly = true;
            this.t1.Width = 60;
            // 
            // t2
            // 
            this.t2.DataPropertyName = "t2";
            this.t2.HeaderText = "温度2";
            this.t2.Name = "t2";
            this.t2.ReadOnly = true;
            this.t2.Width = 60;
            // 
            // t3
            // 
            this.t3.DataPropertyName = "t3";
            this.t3.HeaderText = "温度3";
            this.t3.Name = "t3";
            this.t3.ReadOnly = true;
            this.t3.Width = 60;
            // 
            // t4
            // 
            this.t4.DataPropertyName = "t4";
            this.t4.HeaderText = "温度4";
            this.t4.Name = "t4";
            this.t4.ReadOnly = true;
            this.t4.Width = 60;
            // 
            // t5
            // 
            this.t5.DataPropertyName = "t5";
            this.t5.HeaderText = "温度5";
            this.t5.Name = "t5";
            this.t5.ReadOnly = true;
            this.t5.Width = 60;
            // 
            // t6
            // 
            this.t6.DataPropertyName = "t6";
            this.t6.HeaderText = "温度6";
            this.t6.Name = "t6";
            this.t6.ReadOnly = true;
            this.t6.Width = 60;
            // 
            // t7
            // 
            this.t7.DataPropertyName = "t7";
            this.t7.HeaderText = "温度7";
            this.t7.Name = "t7";
            this.t7.ReadOnly = true;
            this.t7.Width = 60;
            // 
            // t8
            // 
            this.t8.DataPropertyName = "t8";
            this.t8.HeaderText = "温度8";
            this.t8.Name = "t8";
            this.t8.ReadOnly = true;
            this.t8.Width = 60;
            // 
            // t9
            // 
            this.t9.DataPropertyName = "t9";
            this.t9.HeaderText = "温度9";
            this.t9.Name = "t9";
            this.t9.ReadOnly = true;
            this.t9.Width = 60;
            // 
            // t10
            // 
            this.t10.DataPropertyName = "t10";
            this.t10.HeaderText = "温度10";
            this.t10.Name = "t10";
            this.t10.ReadOnly = true;
            this.t10.Width = 66;
            // 
            // t11
            // 
            this.t11.DataPropertyName = "t11";
            this.t11.HeaderText = "温度11";
            this.t11.Name = "t11";
            this.t11.ReadOnly = true;
            this.t11.Width = 66;
            // 
            // t12
            // 
            this.t12.DataPropertyName = "t12";
            this.t12.HeaderText = "温度12";
            this.t12.Name = "t12";
            this.t12.ReadOnly = true;
            this.t12.Width = 66;
            // 
            // t13
            // 
            this.t13.DataPropertyName = "t13";
            this.t13.HeaderText = "温度13";
            this.t13.Name = "t13";
            this.t13.ReadOnly = true;
            this.t13.Width = 66;
            // 
            // MutlipleTemp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.ClientSize = new System.Drawing.Size(907, 604);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox3);
            this.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MutlipleTemp";
            this.Text = "温度报表";
           // this.Load += new System.EventHandler(this.MutlipleTemp_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AddDatetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn t1;
        private System.Windows.Forms.DataGridViewTextBoxColumn t2;
        private System.Windows.Forms.DataGridViewTextBoxColumn t3;
        private System.Windows.Forms.DataGridViewTextBoxColumn t4;
        private System.Windows.Forms.DataGridViewTextBoxColumn t5;
        private System.Windows.Forms.DataGridViewTextBoxColumn t6;
        private System.Windows.Forms.DataGridViewTextBoxColumn t7;
        private System.Windows.Forms.DataGridViewTextBoxColumn t8;
        private System.Windows.Forms.DataGridViewTextBoxColumn t9;
        private System.Windows.Forms.DataGridViewTextBoxColumn t10;
        private System.Windows.Forms.DataGridViewTextBoxColumn t11;
        private System.Windows.Forms.DataGridViewTextBoxColumn t12;
        private System.Windows.Forms.DataGridViewTextBoxColumn t13;

    }
}