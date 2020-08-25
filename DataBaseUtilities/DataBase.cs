using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Services;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Common;

namespace DataBaseUtilities
{
    public class DataBase
    {
        public static bool Finish_Event;
        public static async Task<ReturnedSaveFuncInfo> BackUpStartAsync(string connectionString, ENSource Source, string path = "", Guid? Guid = null, CancellationToken token = default)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {
                token.ThrowIfCancellationRequested();
                if (path == "")
                {
                    token.ThrowIfCancellationRequested();
                    var dlg = new SaveFileDialog { Title = @"پشتیبان گیری اطلاعات آراد" };
                    token.ThrowIfCancellationRequested();
                    var file = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly()?.Location)
                        ?.Replace(".exe", "__");
                    var d = Calendar.MiladiToShamsi(DateTime.Now).Replace("/", "_");
                    d += "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
                    file += d;
                    file = file.Replace(" ", "");
                    dlg.FileName = file;
                    token.ThrowIfCancellationRequested();
                    dlg.Filter = "*.NPZ2|*.NPZ2";
                    token.ThrowIfCancellationRequested();
                    token.ThrowIfCancellationRequested();
                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        ret.AddReturnedValue(ReturnedState.Warning, "لغو  توسط کاربر. عدم انتخاب آدرس ذخیره سازی.");
                        return ret;
                    }
                    path = dlg.FileName;
                }
                token.ThrowIfCancellationRequested();
                ret.AddReturnedValue(await DatabaseAction.BackupDbAsync(connectionString, Source, path, Guid, token));
            }
            catch (ThreadAbortException ex) { ret.AddReturnedValue(ex); }
            catch (OperationCanceledException ex) { ret.AddReturnedValue(ex); }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                Finish_Event = true;
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> ReStoreStartAsync(string connectionString, ENSource Source, string path = "", bool autoBackup = true)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    var ofd = new OpenFileDialog
                    {
                        Multiselect = false,
                        Filter = @"Backup Files(*.NPZ;*.NPZ2;*.BAK)|*.NPZ;*.NPZ2;*.BAK",
                        Title = @"فایل حاوی اطلاعات پشتیبانی نرم افزار را انتخاب نمائید"
                    };

                    if (ofd.ShowDialog() != DialogResult.OK)
                    {
                        ret.AddReturnedValue(ReturnedState.Warning, "بازگردانی اطلاعات توسط کاربر لغو شد. عدم انتخاب آدرس ذخیره سازی فایل.");
                        return ret;
                    }
                    path = ofd.FileName;
                }

                var backUpVersion = GetBackUpVersion(connectionString, path);
                var dataBaseVersion = GetDataBaseVersion(connectionString);
                if (backUpVersion > dataBaseVersion)
                {
                    ret.AddReturnedValue(ReturnedState.Error, $@"{backUpVersion} نسخه فایل پشتیبان" + " \r\n" +
                                    $@"{dataBaseVersion} نسخه دیتابیس" + "\r\n" +
                                    "بدلیل بالاتر بودن نسخه پشتیبان نسبت به دیتابیس، امکان بازگردانی وجود ندارد");
                    return ret;
                }

                SqlConnection.ClearAllPools();
                ret.AddReturnedValue(await DatabaseAction.ReStoreDbAsync(connectionString, path, autoBackup, Source));
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        private static float GetBackUpVersion(string connctionString, string backUpFileName)
        {
            var ver = (float)0;
            var line = 0;
            try
            {
                var cn = new SqlConnection(connctionString);

                line = 1;
                if (backUpFileName.EndsWith(".NPZ") || backUpFileName.EndsWith(".npz"))
                {
                    var zp = new Zip();
                    line = 2;
                    backUpFileName = zp.ExtractTempDIR(backUpFileName);
                }


                else if (backUpFileName.EndsWith(".NPZ2") || backUpFileName.EndsWith(".npz2"))
                {
                    try
                    {
                        line = 3;
                        var pathtemp = Zip.TempDirName();
                        line = 4;
                        var fileInfo = new FileInfo(backUpFileName);
                        line = 5;
                        pathtemp = Path.Combine(pathtemp, fileInfo.Name);
                        line = 6;
                        pathtemp = Path.ChangeExtension(pathtemp, ".bak");
                        line = 7;
                        using (var archive = GZipArchive.Open(backUpFileName))
                        {
                            line = 8;
                            foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                            {
                                line = 9;
                                entry.WriteToFile(pathtemp, new ExtractionOptions()
                                {
                                    ExtractFullPath = true,
                                    Overwrite = true
                                });
                            }
                        }

                        line = 10;
                        backUpFileName = pathtemp;
                    }
                    catch
                    {
                        var zp = new Zip();
                        line = 2000;
                        backUpFileName = zp.ExtractTempDIR(backUpFileName);
                    }
                }

                line = 11;
                if (string.IsNullOrEmpty(backUpFileName)) return ver;
                line = 12;
                var command = @"RESTORE HEADERONLY FROM DISK ='" + backUpFileName + "'";
                line = 13;

                using (var sqlCommand = new SqlCommand(command, cn))
                {
                    line = 14;
                    cn.Open();
                    line = 15;
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    line = 16;
                    while (sqlDataReader.Read())
                    {
                        line = 17;
                        Console.WriteLine();
                        line = 18;
                        var d = $"{sqlDataReader["DatabaseVersion"]}";
                        line = 19;
                        if (string.IsNullOrEmpty(d)) continue;
                        line = 20;
                        switch (d)
                        {
                            case "406": ver = 6; break;
                            case "408": ver = (float)6.5; break;
                            case "515": ver = 7; break;
                            case "539": ver = 2000; break;
                            case "611": ver = 2005; break;
                            case "612": ver = 2005; break;
                            case "655": ver = 2008; break;
                            case "661": ver = 2008; break;
                            case "706": ver = 2012; break;
                            case "782": ver = 2014; break;
                            case "869": ver = 2017; break;
                            case "895": ver = 2019; break;
                            case "852": ver = 2016; break;
                            default: ver = 0; break;
                        }

                        line = 21;
                        return ver;
                    }
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex,
                    $"Error In Line{line} With ConnectionString:{connctionString} AND BackUpFileName:{backUpFileName}");
                ver = 0;
            }

            return ver;
        }

        private static float GetDataBaseVersion(string connctionString)
        {
            var errMsg = $"connctionString = {connctionString} ";

            var ver = (float)0;
            try
            {
                var cn = new SqlConnection(connctionString);
                var query = "SELECT case" +
                           " when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '8.0%' then 'SQL Server 2000'" +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '9.0%' then 'SQL Server 2005'" +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '10.0%' then 'SQL Server 2008' " +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '10.5%' then 'SQL Server 2008 R2' " +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '11.0%' then 'SQL Server 2012' " +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '12.0%' then 'SQL Server 2014' " +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '13.0%' then 'SQL Server 2016' " +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "like '14.0%' then 'SQL Server 2017' " +
                           "when CONVERT(sysname, SERVERPROPERTY('ProductVersion')) " +
                           "> '14.0.9' then 'SQL Server 2019 RC' " +
                           "else 'unknown' end as [version]";
                var da = new SqlDataAdapter(query, cn);
                var dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count <= 0) ver = 0;
                errMsg += $"fullVersionName = {dt.Rows[0].ItemArray[0]} ";
                //var versionName = dt.Rows[0].ItemArray[0].ToString().Remove(0, 11);
                var versionName = dt.Rows[0].ItemArray[0].ToString()
                    .Replace("SQL Server", "")
                    .Replace("RC", "")
                    .Replace("R2", "")
                    .Trim();
                errMsg += $"VersionName = {versionName} ";
                ver = float.Parse(versionName);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex, errMsg);
                ver = 0;
            }

            return ver;

        }

        //public static void FRMAutobackUp(bool ShowForm)
        //{
        //    FRMExitAutoBackup frmautobackup = new FRMExitAutoBackup();
        //    //if (!ShowForm)
        //    //{
        //    //    frmautobackup.ShowDialog(this);
        //    //    if (AutoDispose)
        //    //        frm.Dispose();
        //    //}
        //    //else
        //    //{
        //    //    frm.ShowInTaskbar = true;
        //    //    frm.Show();
        //    //}

        //    frmautobackup.ShowDialog(this);
        //    //if (frmautobackup.DialogResult == DialogResult.OK)
        //    //{
        //    //    return true;
        //    //}

        //    //return false;
        //}




        public static string GetServerConnectionString()
        {
            try
            {
                return Settings.ServerConnectionsString;
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }

            return "";
        }
        public static string ConnectionString
        {
            get => Settings.ConnectionString;
            set => Settings.ConnectionString = value;
        }

        public static CreateDatabaseResult CreateDatabase(string dataBaseName, string serverName)
        {
            var ret = new CreateDatabaseResult();
            if (dataBaseName == "")
            {
                ret.Status = EnCreateDataBase.DatabaseNameEmpty;
                ret.Result.AddReturnedValue(ReturnedState.Error, $"نام بانک اطلاعاتی خالی است.");
                return ret;
            }

            if (string.IsNullOrEmpty(serverName))
            {
                ret.Status = EnCreateDataBase.ServerConnectionStringError;
                ret.Result.AddReturnedValue(ReturnedState.Error, $"نام سروریس دهنده sql خالی است.");
                return ret;
            }

            try
            {
                return DatabaseAction.CreateDataBase(dataBaseName, serverName);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.Result.AddReturnedValue(ex);
            }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> SetRegistery(string sqlConnection, string name)
        {
            var ret = new ReturnedSaveFuncInfo();
            try
            {
                Microsoft.Win32.Registry.SetValue("HKEY_CURRENT_USER\\Software\\Arad\\" + name + "", "SQLCN",
                    sqlConnection);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public static async Task MakeDataBase(string dbName, IWin32Window owner)
        {
            try
            {
                var res = DatabaseAction.CreateDataBase(dbName);
                if (res.Status == EnCreateDataBase.ServerConnectionStringError)
                {
                    MessageBox.Show(owner, $"سرویس sql در دسترس نمیباشد\r\n{res.Result.ErrorMessage}", "پیغام سیستم");
                    return;
                }

                for (var i = 0; i < 20; i++)
                {
                    res = DatabaseAction.CreateDataBase($"{dbName}_{i}");
                    if (res.Status != EnCreateDataBase.Success) continue;
                    var resSetConnection =
                        await DataBase.SetRegistery(res.ConnectionString, dbName);
                    if (!resSetConnection.HasError) return;
                    return;
                }

                if (res.Result.HasError)
                {
                }
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }
    }
}
