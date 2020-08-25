namespace DataBaseUtilities
{
    partial class FRMInitialSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRMInitialSettings));
            this.grp = new DevComponents.DotNetBar.PanelEx();
            this.PanelDB = new System.Windows.Forms.Panel();
            this.Label5 = new System.Windows.Forms.Label();
            this.InitialCategory = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.DataSource = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.IntegratedSecuriry = new System.Windows.Forms.CheckBox();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Label4 = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Pass = new System.Windows.Forms.TextBox();
            this.btnCancel = new DevComponents.DotNetBar.ButtonX();
            this.btnTest = new DevComponents.DotNetBar.ButtonX();
            this.btnFinish = new DevComponents.DotNetBar.ButtonX();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.grp.SuspendLayout();
            this.PanelDB.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grp
            // 
            this.grp.CanvasColor = System.Drawing.SystemColors.Control;
            this.grp.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.grp.Controls.Add(this.PanelDB);
            this.grp.Controls.Add(this.Label2);
            this.grp.Controls.Add(this.GroupBox1);
            this.grp.Controls.Add(this.GroupBox2);
            this.grp.DisabledBackColor = System.Drawing.Color.Empty;
            this.grp.Location = new System.Drawing.Point(6, 19);
            this.grp.Name = "grp";
            this.grp.Size = new System.Drawing.Size(425, 398);
            this.grp.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.grp.Style.BackColor1.Color = System.Drawing.Color.White;
            this.grp.Style.BackColor2.Color = System.Drawing.Color.White;
            this.grp.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.grp.Style.BorderColor.Color = System.Drawing.Color.Silver;
            this.grp.Style.BorderWidth = 2;
            this.grp.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded;
            this.grp.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.grp.Style.GradientAngle = 90;
            this.grp.TabIndex = 0;
            // 
            // PanelDB
            // 
            this.PanelDB.Controls.Add(this.Label5);
            this.PanelDB.Controls.Add(this.InitialCategory);
            this.PanelDB.Controls.Add(this.TextBox1);
            this.PanelDB.Location = new System.Drawing.Point(13, 337);
            this.PanelDB.Name = "PanelDB";
            this.PanelDB.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.PanelDB.Size = new System.Drawing.Size(396, 46);
            this.PanelDB.TabIndex = 2;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(289, 11);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(96, 20);
            this.Label5.TabIndex = 1;
            this.Label5.Text = "نام بانك اطلاعاتي :";
            // 
            // InitialCategory
            // 
            this.InitialCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InitialCategory.FormattingEnabled = true;
            this.InitialCategory.Items.AddRange(new object[] {
            "Hesab88"});
            this.InitialCategory.Location = new System.Drawing.Point(154, 8);
            this.InitialCategory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InitialCategory.Name = "InitialCategory";
            this.InitialCategory.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.InitialCategory.Size = new System.Drawing.Size(127, 28);
            this.InitialCategory.TabIndex = 0;
            this.InitialCategory.Enter += new System.EventHandler(this.InitialCategory_Enter);
            // 
            // Label2
            // 
            this.Label2.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Label2.Location = new System.Drawing.Point(12, 14);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(401, 58);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "كاربر گرامي ، \r\nتوصيه ميشود جهت تنظيم پارامترهاي اين قسمت از تيم فني گروه نرم افز" +
    "اري آراد كمك بگيريد";
            // 
            // GroupBox1
            // 
            this.GroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.DataSource);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.GroupBox1.Location = new System.Drawing.Point(13, 77);
            this.GroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GroupBox1.Size = new System.Drawing.Size(396, 140);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "مشخصات سرور";
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Label3.ForeColor = System.Drawing.Color.Blue;
            this.Label3.Location = new System.Drawing.Point(14, 63);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(365, 72);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "1- در صورتي كه سيستم در شبكه كار ميكند اين بخش را بايد پس از راه اندازي شبكه به ص" +
    "ورت ServerIP\\SQLExpress پر كنيد\r\n2- و اگر سيستم بدون شبكه كار ميكند با .\\SQLExpr" +
    "ess";
            // 
            // DataSource
            // 
            this.DataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataSource.FormattingEnabled = true;
            this.DataSource.Items.AddRange(new object[] {
            ".\\SQLExpress",
            ".",
            "(Local)"});
            this.DataSource.Location = new System.Drawing.Point(116, 28);
            this.DataSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DataSource.Name = "DataSource";
            this.DataSource.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.DataSource.Size = new System.Drawing.Size(220, 28);
            this.DataSource.TabIndex = 0;
            this.DataSource.SelectedIndexChanged += new System.EventHandler(this.DataSource_SelectedIndexChanged);
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(344, 30);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(49, 28);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "نام سرور :";
            // 
            // GroupBox2
            // 
            this.GroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.GroupBox2.Controls.Add(this.Panel1);
            this.GroupBox2.Controls.Add(this.IntegratedSecuriry);
            this.GroupBox2.Location = new System.Drawing.Point(13, 227);
            this.GroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GroupBox2.Size = new System.Drawing.Size(396, 102);
            this.GroupBox2.TabIndex = 1;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "احراز هويت";
            // 
            // IntegratedSecuriry
            // 
            this.IntegratedSecuriry.AutoSize = true;
            this.IntegratedSecuriry.Checked = true;
            this.IntegratedSecuriry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IntegratedSecuriry.Location = new System.Drawing.Point(173, 25);
            this.IntegratedSecuriry.Name = "IntegratedSecuriry";
            this.IntegratedSecuriry.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.IntegratedSecuriry.Size = new System.Drawing.Size(203, 24);
            this.IntegratedSecuriry.TabIndex = 0;
            this.IntegratedSecuriry.Text = "احراز هويت توسط ويندوز انجام شود";
            this.IntegratedSecuriry.UseVisualStyleBackColor = true;
            this.IntegratedSecuriry.CheckedChanged += new System.EventHandler(this.IntegratedSecuriry_CheckedChanged);
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.Pass);
            this.Panel1.Controls.Add(this.Label6);
            this.Panel1.Controls.Add(this.UserName);
            this.Panel1.Controls.Add(this.Label4);
            this.Panel1.Enabled = false;
            this.Panel1.Location = new System.Drawing.Point(5, 53);
            this.Panel1.Name = "Panel1";
            this.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Panel1.Size = new System.Drawing.Size(391, 41);
            this.Panel1.TabIndex = 1;
            // 
            // Label4
            // 
            this.Label4.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Label4.Location = new System.Drawing.Point(337, 8);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(49, 27);
            this.Label4.TabIndex = 0;
            this.Label4.Text = "نام كاربر :";
            // 
            // UserName
            // 
            this.UserName.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.UserName.Location = new System.Drawing.Point(202, 5);
            this.UserName.Name = "UserName";
            this.UserName.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.UserName.Size = new System.Drawing.Size(129, 27);
            this.UserName.TabIndex = 0;
            // 
            // Label6
            // 
            this.Label6.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Label6.Location = new System.Drawing.Point(131, 8);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(52, 27);
            this.Label6.TabIndex = 2;
            this.Label6.Text = "كلمه عبور :";
            // 
            // Pass
            // 
            this.Pass.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Pass.Location = new System.Drawing.Point(7, 5);
            this.Pass.Name = "Pass";
            this.Pass.PasswordChar = '*';
            this.Pass.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Pass.Size = new System.Drawing.Size(118, 27);
            this.Pass.TabIndex = 1;
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
            this.btnCancel.Location = new System.Drawing.Point(44, 429);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btnCancel.Size = new System.Drawing.Size(103, 31);
            this.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "انصراف";
            this.btnCancel.TextColor = System.Drawing.Color.White;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnTest
            // 
            this.btnTest.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.BackColor = System.Drawing.Color.DarkSlateGray;
            this.btnTest.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTest.ColorTable = DevComponents.DotNetBar.eButtonColor.Flat;
            this.btnTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTest.Font = new System.Drawing.Font("B Yekan", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.btnTest.Location = new System.Drawing.Point(284, 429);
            this.btnTest.Name = "btnTest";
            this.btnTest.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btnTest.Size = new System.Drawing.Size(103, 31);
            this.btnTest.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "تست ارتباط";
            this.btnTest.TextColor = System.Drawing.Color.White;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
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
            this.btnFinish.Location = new System.Drawing.Point(164, 429);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(10);
            this.btnFinish.Size = new System.Drawing.Size(103, 31);
            this.btnFinish.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2013;
            this.btnFinish.TabIndex = 2;
            this.btnFinish.Text = "تایید";
            this.btnFinish.TextColor = System.Drawing.Color.White;
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // TextBox1
            // 
            this.TextBox1.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.TextBox1.Location = new System.Drawing.Point(12, 11);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBox1.Size = new System.Drawing.Size(118, 27);
            this.TextBox1.TabIndex = 0;
            this.TextBox1.Visible = false;
            // 
            // FRMInitialSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 470);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.grp);
            this.Font = new System.Drawing.Font("B Yekan", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FRMInitialSettings";
            this.Padding = new System.Windows.Forms.Padding(27, 92, 27, 31);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Load += new System.EventHandler(this.FRMInitialSettings_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FRMInitialSettings_KeyDown);
            this.grp.ResumeLayout(false);
            this.PanelDB.ResumeLayout(false);
            this.PanelDB.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx grp;
        internal System.Windows.Forms.Panel PanelDB;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.ComboBox InitialCategory;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.ComboBox DataSource;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.TextBox Pass;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox UserName;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.CheckBox IntegratedSecuriry;
        private DevComponents.DotNetBar.ButtonX btnCancel;
        private DevComponents.DotNetBar.ButtonX btnTest;
        private DevComponents.DotNetBar.ButtonX btnFinish;
        internal System.Windows.Forms.TextBox TextBox1;
    }
}