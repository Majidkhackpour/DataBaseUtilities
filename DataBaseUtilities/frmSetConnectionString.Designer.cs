namespace DataBaseUtilities
{
    partial class frmSetConnectionString
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetConnectionString));
            this.grpNetwork = new System.Windows.Forms.GroupBox();
            this.BTN_NetIntial = new System.Windows.Forms.Button();
            this.rbtnNetwork = new System.Windows.Forms.RadioButton();
            this.rbtnCreate = new System.Windows.Forms.RadioButton();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Txt_PathConnection = new System.Windows.Forms.TextBox();
            this.BTN_Intial = new System.Windows.Forms.Button();
            this.Txt_DatabaseName = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnFinish = new DevComponents.DotNetBar.ButtonX();
            this.grpNetwork.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpNetwork
            // 
            this.grpNetwork.Controls.Add(this.BTN_NetIntial);
            this.grpNetwork.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.grpNetwork.Location = new System.Drawing.Point(11, 54);
            this.grpNetwork.Name = "grpNetwork";
            this.grpNetwork.Size = new System.Drawing.Size(402, 79);
            this.grpNetwork.TabIndex = 2;
            this.grpNetwork.TabStop = false;
            this.grpNetwork.Text = "تنظیمات رشته اتصال";
            // 
            // BTN_NetIntial
            // 
            this.BTN_NetIntial.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold);
            this.BTN_NetIntial.Location = new System.Drawing.Point(9, 32);
            this.BTN_NetIntial.Name = "BTN_NetIntial";
            this.BTN_NetIntial.Size = new System.Drawing.Size(381, 38);
            this.BTN_NetIntial.TabIndex = 0;
            this.BTN_NetIntial.Text = "تنظیم رشته ی اتصال";
            this.BTN_NetIntial.UseVisualStyleBackColor = true;
            this.BTN_NetIntial.Click += new System.EventHandler(this.BTN_NetIntial_Click);
            // 
            // rbtnNetwork
            // 
            this.rbtnNetwork.AutoSize = true;
            this.rbtnNetwork.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold);
            this.rbtnNetwork.Location = new System.Drawing.Point(42, 23);
            this.rbtnNetwork.Name = "rbtnNetwork";
            this.rbtnNetwork.Size = new System.Drawing.Size(191, 27);
            this.rbtnNetwork.TabIndex = 1;
            this.rbtnNetwork.TabStop = true;
            this.rbtnNetwork.Text = "تنظیمات رشته اتصال  پیشرفته/شبکه";
            this.rbtnNetwork.UseVisualStyleBackColor = true;
            this.rbtnNetwork.CheckedChanged += new System.EventHandler(this.rbtnNetwork_CheckedChanged);
            // 
            // rbtnCreate
            // 
            this.rbtnCreate.AutoSize = true;
            this.rbtnCreate.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold);
            this.rbtnCreate.Location = new System.Drawing.Point(259, 23);
            this.rbtnCreate.Name = "rbtnCreate";
            this.rbtnCreate.Size = new System.Drawing.Size(144, 27);
            this.rbtnCreate.TabIndex = 0;
            this.rbtnCreate.Text = "بانک اطلاعاتی ایجاد شود";
            this.rbtnCreate.UseVisualStyleBackColor = true;
            this.rbtnCreate.CheckedChanged += new System.EventHandler(this.rbtnCreate_CheckedChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.Txt_PathConnection);
            this.GroupBox1.Controls.Add(this.BTN_Intial);
            this.GroupBox1.Controls.Add(this.Txt_DatabaseName);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.GroupBox1.Location = new System.Drawing.Point(7, 56);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(402, 200);
            this.GroupBox1.TabIndex = 19;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "تنظیمات بانک";
            // 
            // Txt_PathConnection
            // 
            this.Txt_PathConnection.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Txt_PathConnection.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Txt_PathConnection.ForeColor = System.Drawing.Color.Yellow;
            this.Txt_PathConnection.Location = new System.Drawing.Point(11, 59);
            this.Txt_PathConnection.Multiline = true;
            this.Txt_PathConnection.Name = "Txt_PathConnection";
            this.Txt_PathConnection.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Txt_PathConnection.Size = new System.Drawing.Size(385, 99);
            this.Txt_PathConnection.TabIndex = 13;
            // 
            // BTN_Intial
            // 
            this.BTN_Intial.Location = new System.Drawing.Point(11, 164);
            this.BTN_Intial.Name = "BTN_Intial";
            this.BTN_Intial.Size = new System.Drawing.Size(385, 27);
            this.BTN_Intial.TabIndex = 0;
            this.BTN_Intial.Text = "تنظیم رشته ی اتصال";
            this.BTN_Intial.UseVisualStyleBackColor = true;
            this.BTN_Intial.Click += new System.EventHandler(this.BTN_Intial_Click);
            // 
            // Txt_DatabaseName
            // 
            this.Txt_DatabaseName.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold);
            this.Txt_DatabaseName.Location = new System.Drawing.Point(15, 26);
            this.Txt_DatabaseName.Name = "Txt_DatabaseName";
            this.Txt_DatabaseName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Txt_DatabaseName.Size = new System.Drawing.Size(223, 27);
            this.Txt_DatabaseName.TabIndex = 0;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("B Titr", 9.75F, System.Drawing.FontStyle.Bold);
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(276, 30);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(101, 23);
            this.Label1.TabIndex = 3;
            this.Label1.Text = "نام بانک اطلاعاتی :";
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.DarkSlateGray;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("B Yekan", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnCancel.Location = new System.Drawing.Point(11, 267);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btnCancel.Size = new System.Drawing.Size(103, 31);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.TextColor = System.Drawing.Color.White;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnFinish
            // 
            this.btnFinish.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.BackColor = System.Drawing.Color.DarkSlateGray;
            this.btnFinish.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFinish.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnFinish.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFinish.Font = new System.Drawing.Font("B Yekan", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnFinish.Location = new System.Drawing.Point(310, 267);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btnFinish.Size = new System.Drawing.Size(103, 31);
            this.btnFinish.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this.btnFinish.TabIndex = 3;
            this.btnFinish.Text = "تایید";
            this.btnFinish.TextColor = System.Drawing.Color.White;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // frmSetConnectionString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 316);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.grpNetwork);
            this.Controls.Add(this.rbtnNetwork);
            this.Controls.Add(this.rbtnCreate);
            this.Controls.Add(this.GroupBox1);
            this.Font = new System.Drawing.Font("B Yekan", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetConnectionString";
            this.Padding = new System.Windows.Forms.Padding(27, 92, 27, 31);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Load += new System.EventHandler(this.frmSetConnectionString_Load);
            this.grpNetwork.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox grpNetwork;
        internal System.Windows.Forms.Button BTN_NetIntial;
        public System.Windows.Forms.RadioButton rbtnNetwork;
        public System.Windows.Forms.RadioButton rbtnCreate;
        internal System.Windows.Forms.GroupBox GroupBox1;
        public System.Windows.Forms.TextBox Txt_PathConnection;
        public System.Windows.Forms.Button BTN_Intial;
        public System.Windows.Forms.TextBox Txt_DatabaseName;
        internal System.Windows.Forms.Label Label1;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnFinish;
    }
}