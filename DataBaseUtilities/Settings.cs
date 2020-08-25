using System;
using Services;

namespace BackUpDLL
{
    public class DataBaseUtilities
    {
        public static string ConnectionString = "";
        private static string _ServerConnectionsString;
        public static string _destinationFileFolder = "";
        private static string FileName => System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "cn_Server.txt");

        public static string ServerConnectionsString
        {
            get
            {
                try
                {
                    if (_ServerConnectionsString == null)
                    {
                        if (System.IO.File.Exists(FileName))
                            _ServerConnectionsString = System.IO.File.ReadAllText(FileName);
                        else
                        {
                            _ServerConnectionsString = "Data Source=.\\SQLEXPRESS;Integrated Security=True;MultipleActiveResultSets=True";
                            System.IO.File.Create(FileName).Close();
                            System.IO.File.WriteAllText(FileName, _ServerConnectionsString);
                        }
                    }
                    return _ServerConnectionsString;
                }
                catch (Exception ex)
                {
                    WebErrorLog.ErrorInstence.StartErrorLog(ex);
                    return "";
                }
            }
            set
            {
                try
                {
                    _ServerConnectionsString = value;
                    if (!System.IO.File.Exists(FileName))
                        System.IO.File.Create(FileName).Close();
                    System.IO.File.WriteAllText(FileName, _ServerConnectionsString);
                }
                catch (Exception ex)
                {
                    WebErrorLog.ErrorInstence.StartErrorLog(ex);
                }
            }
        }

        public static bool SetDistanationFolder(string ServerConnection)
        {
            try
            {
                string path = System.IO.Path.Combine(System.Windows.Forms.Application.StartupPath, "Database");
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                if (path.ToLower().StartsWith(@"c:\") || path.ToLower().StartsWith(@"d:\projects"))
                {
                    //    'به علت جلوگیری از تنظیمات دسترسی سرویس SQL
                    //'به پوشه دیتابیس ها در درایو c
                    //'برای درایو c
                    //'این عملیات را انجام نمیدهیم
                    return false;
                }

                System.Data.SqlClient.SqlConnection cn = new System.Data.SqlClient.SqlConnection(ServerConnection);
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("", cn);
                cn.Open();
                path = "N'" + path + "'";
                cmd.CommandText = @"EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultLog', REG_SZ, " + path;
                cmd.ExecuteNonQuery();
                cmd.CommandText = @"EXEC xp_instance_regwrite N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultData', REG_SZ," + path;
                cmd.ExecuteNonQuery();
                cn.Close();
                return true;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                return false;
            }
        }
    }
}
