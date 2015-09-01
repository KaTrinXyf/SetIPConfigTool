namespace SetIPConfigTool
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label_NetCard = new System.Windows.Forms.Label();
            this.label_IPInfo = new System.Windows.Forms.Label();
            this.comboBox_NetCard = new System.Windows.Forms.ComboBox();
            this.btn_Update = new System.Windows.Forms.Button();
            this.btn_CopyIPInfo = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.labe_GateWay = new System.Windows.Forms.Label();
            this.label_addIP = new System.Windows.Forms.Label();
            this.textBox_GateWay = new System.Windows.Forms.TextBox();
            this.textBox_AddIP = new System.Windows.Forms.TextBox();
            this.label_NetMask = new System.Windows.Forms.Label();
            this.textBox_NetMask = new System.Windows.Forms.TextBox();
            this.btn_AddIP = new System.Windows.Forms.Button();
            this.btn_Set = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.label_underline = new System.Windows.Forms.Label();
            this.textBox_EndNet = new System.Windows.Forms.TextBox();
            this.labelunderline = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label_NetCard
            // 
            this.label_NetCard.AutoSize = true;
            this.label_NetCard.Location = new System.Drawing.Point(12, 21);
            this.label_NetCard.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_NetCard.Name = "label_NetCard";
            this.label_NetCard.Size = new System.Drawing.Size(47, 12);
            this.label_NetCard.TabIndex = 0;
            this.label_NetCard.Text = "网卡 ：";
            // 
            // label_IPInfo
            // 
            this.label_IPInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_IPInfo.AutoSize = true;
            this.label_IPInfo.Location = new System.Drawing.Point(7, 63);
            this.label_IPInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_IPInfo.Name = "label_IPInfo";
            this.label_IPInfo.Size = new System.Drawing.Size(59, 12);
            this.label_IPInfo.TabIndex = 4;
            this.label_IPInfo.Text = "IP信息 ：";
            // 
            // comboBox_NetCard
            // 
            this.comboBox_NetCard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_NetCard.FormattingEnabled = true;
            this.comboBox_NetCard.Location = new System.Drawing.Point(91, 17);
            this.comboBox_NetCard.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.comboBox_NetCard.Name = "comboBox_NetCard";
            this.comboBox_NetCard.Size = new System.Drawing.Size(304, 20);
            this.comboBox_NetCard.TabIndex = 1;
            this.comboBox_NetCard.SelectedIndexChanged += new System.EventHandler(this.comboBox_NetCard_SelectedIndexChanged);
            // 
            // btn_Update
            // 
            this.btn_Update.Location = new System.Drawing.Point(413, 16);
            this.btn_Update.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Update.Name = "btn_Update";
            this.btn_Update.Size = new System.Drawing.Size(74, 23);
            this.btn_Update.TabIndex = 2;
            this.btn_Update.Text = "刷新";
            this.btn_Update.UseVisualStyleBackColor = true;
            this.btn_Update.Click += new System.EventHandler(this.btn_Update_Click);
            // 
            // btn_CopyIPInfo
            // 
            this.btn_CopyIPInfo.Location = new System.Drawing.Point(506, 16);
            this.btn_CopyIPInfo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_CopyIPInfo.Name = "btn_CopyIPInfo";
            this.btn_CopyIPInfo.Size = new System.Drawing.Size(74, 23);
            this.btn_CopyIPInfo.TabIndex = 3;
            this.btn_CopyIPInfo.Text = "拷贝IP信息";
            this.btn_CopyIPInfo.UseVisualStyleBackColor = true;
            this.btn_CopyIPInfo.Click += new System.EventHandler(this.btn_CopyIPInfo_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(91, 63);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(330, 244);
            this.dataGridView.TabIndex = 5;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            // 
            // labe_GateWay
            // 
            this.labe_GateWay.AutoSize = true;
            this.labe_GateWay.Location = new System.Drawing.Point(9, 321);
            this.labe_GateWay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labe_GateWay.Name = "labe_GateWay";
            this.labe_GateWay.Size = new System.Drawing.Size(47, 12);
            this.labe_GateWay.TabIndex = 6;
            this.labe_GateWay.Text = "网关 ：";
            // 
            // label_addIP
            // 
            this.label_addIP.AutoSize = true;
            this.label_addIP.Location = new System.Drawing.Point(9, 363);
            this.label_addIP.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_addIP.Name = "label_addIP";
            this.label_addIP.Size = new System.Drawing.Size(59, 12);
            this.label_addIP.TabIndex = 8;
            this.label_addIP.Text = "新增IP ：";
            // 
            // textBox_GateWay
            // 
            this.textBox_GateWay.Location = new System.Drawing.Point(88, 317);
            this.textBox_GateWay.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_GateWay.Name = "textBox_GateWay";
            this.textBox_GateWay.Size = new System.Drawing.Size(151, 21);
            this.textBox_GateWay.TabIndex = 7;
            this.textBox_GateWay.Text = "暂无网关信息";
            // 
            // textBox_AddIP
            // 
            this.textBox_AddIP.Location = new System.Drawing.Point(88, 359);
            this.textBox_AddIP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_AddIP.Name = "textBox_AddIP";
            this.textBox_AddIP.Size = new System.Drawing.Size(120, 21);
            this.textBox_AddIP.TabIndex = 9;
            // 
            // label_NetMask
            // 
            this.label_NetMask.AutoSize = true;
            this.label_NetMask.Location = new System.Drawing.Point(292, 363);
            this.label_NetMask.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_NetMask.Name = "label_NetMask";
            this.label_NetMask.Size = new System.Drawing.Size(71, 12);
            this.label_NetMask.TabIndex = 11;
            this.label_NetMask.Text = "子网掩码 ：";
            // 
            // textBox_NetMask
            // 
            this.textBox_NetMask.Location = new System.Drawing.Point(369, 359);
            this.textBox_NetMask.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_NetMask.Name = "textBox_NetMask";
            this.textBox_NetMask.Size = new System.Drawing.Size(116, 21);
            this.textBox_NetMask.TabIndex = 12;
            // 
            // btn_AddIP
            // 
            this.btn_AddIP.Location = new System.Drawing.Point(501, 359);
            this.btn_AddIP.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_AddIP.Name = "btn_AddIP";
            this.btn_AddIP.Size = new System.Drawing.Size(74, 23);
            this.btn_AddIP.TabIndex = 13;
            this.btn_AddIP.Text = "添加";
            this.btn_AddIP.UseVisualStyleBackColor = true;
            this.btn_AddIP.Click += new System.EventHandler(this.btn_AddIP_Click);
            // 
            // btn_Set
            // 
            this.btn_Set.Location = new System.Drawing.Point(133, 388);
            this.btn_Set.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Set.Name = "btn_Set";
            this.btn_Set.Size = new System.Drawing.Size(74, 23);
            this.btn_Set.TabIndex = 14;
            this.btn_Set.Text = "应用配置";
            this.btn_Set.UseVisualStyleBackColor = true;
            this.btn_Set.Click += new System.EventHandler(this.btn_Set_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(347, 388);
            this.btn_Exit.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(74, 23);
            this.btn_Exit.TabIndex = 15;
            this.btn_Exit.Text = "退出";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // label_underline
            // 
            this.label_underline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_underline.AutoSize = true;
            this.label_underline.Location = new System.Drawing.Point(208, 456);
            this.label_underline.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_underline.Name = "label_underline";
            this.label_underline.Size = new System.Drawing.Size(0, 12);
            this.label_underline.TabIndex = 15;
            // 
            // textBox_EndNet
            // 
            this.textBox_EndNet.Location = new System.Drawing.Point(232, 359);
            this.textBox_EndNet.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.textBox_EndNet.Name = "textBox_EndNet";
            this.textBox_EndNet.Size = new System.Drawing.Size(38, 21);
            this.textBox_EndNet.TabIndex = 10;
            // 
            // labelunderline
            // 
            this.labelunderline.AutoSize = true;
            this.labelunderline.Location = new System.Drawing.Point(213, 362);
            this.labelunderline.Name = "labelunderline";
            this.labelunderline.Size = new System.Drawing.Size(11, 12);
            this.labelunderline.TabIndex = 16;
            this.labelunderline.Text = "-";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(631, 438);
            this.Controls.Add(this.labelunderline);
            this.Controls.Add(this.textBox_EndNet);
            this.Controls.Add(this.label_underline);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.btn_Set);
            this.Controls.Add(this.btn_AddIP);
            this.Controls.Add(this.textBox_NetMask);
            this.Controls.Add(this.label_NetMask);
            this.Controls.Add(this.textBox_AddIP);
            this.Controls.Add(this.textBox_GateWay);
            this.Controls.Add(this.label_addIP);
            this.Controls.Add(this.labe_GateWay);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.btn_CopyIPInfo);
            this.Controls.Add(this.btn_Update);
            this.Controls.Add(this.comboBox_NetCard);
            this.Controls.Add(this.label_IPInfo);
            this.Controls.Add(this.label_NetCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "IP编辑器";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_NetCard;
        private System.Windows.Forms.Label label_IPInfo;
        private System.Windows.Forms.ComboBox comboBox_NetCard;
        private System.Windows.Forms.Button btn_Update;
        private System.Windows.Forms.Button btn_CopyIPInfo;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label labe_GateWay;
        private System.Windows.Forms.Label label_addIP;
        private System.Windows.Forms.TextBox textBox_GateWay;
        private System.Windows.Forms.TextBox textBox_AddIP;
        private System.Windows.Forms.Label label_NetMask;
        private System.Windows.Forms.TextBox textBox_NetMask;
        private System.Windows.Forms.Button btn_AddIP;
        private System.Windows.Forms.Button btn_Set;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.Label label_underline;
        private System.Windows.Forms.TextBox textBox_EndNet;
        private System.Windows.Forms.Label labelunderline;
    }
}

