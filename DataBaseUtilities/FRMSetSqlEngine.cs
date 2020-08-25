using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using MetroFramework.Forms;
using Services;

namespace DataBaseUtilities
{
    public partial class FRMSetSqlEngine : MetroForm
    {
        private string _serverConnectionsString = "";
        private SqlConnectionStringBuilder _builder = new SqlConnectionStringBuilder();
        public FRMSetSqlEngine()
        {
            InitializeComponent();
        }

        private void FRMSetSqlEngine_Load(object sender, System.EventArgs e)
        {
            try
            {
                var temp = new SqlConnectionStringBuilder(Settings.ServerConnectionsString);
                TxtServerName.Text = temp.DataSource;
                RBWindowsDetect.Checked = temp.IntegratedSecurity;
                RBSqlServerDetect.Checked = !temp.IntegratedSecurity;
                TxtUserName.Text = temp.UserID;
                TxtPassword.Text = temp.Password;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (!TestConnection(false)) return;
                Settings.ServerConnectionsString = _builder.ConnectionString;
                _serverConnectionsString = _builder.ConnectionString;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        private bool TestConnection(bool swShowDialo)
        {
            try
            {
                _builder.DataSource = TxtServerName.Text;
                if (RBSqlServerDetect.Checked)
                {
                    _builder.UserID = TxtUserName.Text;
                    _builder.Password = TxtPassword.Text;
                    _builder.IntegratedSecurity = false;
                }
                else
                {
                    _builder.UserID = "";
                    _builder.Password = "";
                    _builder.IntegratedSecurity = true;
                }

                _builder.AsynchronousProcessing = true;
                var con = new SqlConnection { ConnectionString = _builder.ConnectionString };
                con.Open();
                if (swShowDialo)
                    MessageBox.Show(this, "ارتباط با بانک اطلاعاتی با موفقیت برقرار شد", "خطا", MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                Settings.ServerConnectionsString = _builder.ConnectionString;
                con.Close();

                return true;

            }
            catch (Exception)
            {
                MessageBox.Show(this, "ارتباط با بانک اطلاعاتی برقرار نیست", "خطا", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                return false;
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                TestConnection(true);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        private void RBSqlServerDetect_CheckedChanged(object sender, EventArgs e)
        {
            TxtPassword.Enabled = TxtUserName.Enabled = RBSqlServerDetect.Checked;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return keyData == (Keys.F6) || base.ProcessCmdKey(ref msg, keyData);
        }

        private void FRMSetSqlEngine_KeyDown(object sender, KeyEventArgs e)
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
    }
}
