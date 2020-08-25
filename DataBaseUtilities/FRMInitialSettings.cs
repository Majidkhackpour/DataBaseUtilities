using System;
using System.Windows.Forms;
using MetroFramework.Forms;
using Services;

namespace DataBaseUtilities
{
    public partial class FRMInitialSettings : MetroForm
    {
        public string ConnectionString { get; private set; } = "";
        public FRMInitialSettings()
        {
            InitializeComponent();
        }
        public FRMInitialSettings(string connectionString)
        {
            InitializeComponent();
            ConnectionString = connectionString;
        }
        private void CreateCN()
        {
            var CSB = new System.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = DataSource.Text,
                IntegratedSecurity = IntegratedSecuriry.Checked
            };
            if (!IntegratedSecuriry.Checked)
            {
                CSB.UserID = UserName.Text;
                CSB.Password = Pass.Text;
            }

            CSB.InitialCatalog = InitialCategory.Text;

            TextBox1.Text = CSB.ConnectionString;
        }

        private void FRMInitialSettings_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F5:
                    btnFinish.PerformClick();
                    break;
                case Keys.Escape:
                    btnCancel.PerformClick();
                    break;
                case Keys.F6:
                    btnTest.PerformClick();
                    break;
            }
        }

        private void FRMInitialSettings_Load(object sender, System.EventArgs e)
        {
            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder csb;
                csb = ConnectionString == ""
                    ? new System.Data.SqlClient.SqlConnectionStringBuilder(Settings.ServerConnectionsString)
                    : new System.Data.SqlClient.SqlConnectionStringBuilder(ConnectionString);
                DataSource.Text = csb.DataSource;
                IntegratedSecuriry.Checked = csb.IntegratedSecurity;
                if (IntegratedSecuriry.Checked)
                    Panel1.Enabled = false;
                else
                {
                    Panel1.Enabled = true;
                    UserName.Text = csb.UserID;
                    Pass.Text = csb.Password;
                }
                InitialCategory.Text = csb.InitialCatalog;

            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InitialCategory.Text))
            {
                MessageBox.Show(this, "بانک اطلاعاتی را باید انتخاب نمایید", "توجه", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return;
            }

            CreateCN();
            var cn = TextBox1.Text;
            try
            {
                var sqlcn = new System.Data.SqlClient.SqlConnection(TextBox1.Text);
                sqlcn.Open();
                sqlcn.Close();
                ConnectionString = sqlcn.ConnectionString;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                MessageBox.Show(this, "امکان باز کردن دیتابیس مورد نظر وجود ندارد");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void IntegratedSecuriry_CheckedChanged(object sender, EventArgs e)
        {
            Panel1.Enabled = IntegratedSecuriry.Checked != true;
            CreateCN();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(InitialCategory.Text))
            {
                MessageBox.Show(this, "بانک اطلاعاتی را باید انتخاب نمایید", "توجه", MessageBoxButtons.OK,
                    MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return;
            }
            CreateCN();
            var cn = new System.Data.SqlClient.SqlConnection(TextBox1.Text);
            try
            {
                cn.Open();
                cn.Close();
                MessageBox.Show(this, "ارتباط با موفقيت برقرار شد", "توجه", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        private void DataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateCN();
        }

        private void InitialCategory_Enter(object sender, EventArgs e)
        {
            try
            {
                InitialCategory.Items.Clear();
                var lst = CLSServerDBs.GetAll(TextBox1.Text);
                if (lst != null && lst.Count > 0)
                {
                    foreach (var item in lst)
                    {
                        if (item.Name.ToLower() != "master" &&
                            item.Name.ToLower() != "model" &&
                            item.Name.ToLower() != "msdb" &&
                            item.Name.ToLower() != "tempdb")
                            InitialCategory.Items.Add(item.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }
    }
}
