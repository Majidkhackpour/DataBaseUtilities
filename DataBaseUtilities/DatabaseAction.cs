using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Services;

namespace BackUpDLL
{
    public class DatabaseAction
    {
        private static CancellationTokenSource DeleteTempFilesToken = new CancellationTokenSource();
        public static event EventHandler<CreateBackupArgs> OnCreateBackup;

        public static string CreateFileName(string connectionString, string directory = "")
        {
            var ret = "";
            try
            {
                var csd = new SqlConnectionStringBuilder(connectionString);
                var FileName =
                    $"{csd.InitialCatalog}__{Calendar.MiladiToShamsi(DateTime.Now).RemoveNoNumbers("-")}__{DateTime.Now.Hour}_{DateTime.Now.Minute}.NP";
                if (string.IsNullOrEmpty(directory))
                    directory = Path.Combine(Application.StartupPath, "TempBackUp");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                ret = Path.Combine(directory, FileName);
            }
            catch (Exception ex) { WebErrorLog.ErrorInstence.StartErrorLog(ex); }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> BackupDbAsync(string connectionString, ENSource Source, string path = "", Guid? Guid = null, CancellationToken token = default)
        {
            var line = 0;
            var res = new ReturnedSaveFuncInfo();
            bool IsAutomatic = string.IsNullOrEmpty(path);
            string DatabaseName = "";
            try
            {
                OnCreateBackup?.Invoke(null, new CreateBackupArgs() { Message = "", State = CreateBackupState.Start });
                token.ThrowIfCancellationRequested();
                DeleteTempFilesToken?.Cancel();

                if (IsAutomatic) path = CreateFileName(connectionString);
                if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    //در نرم افزار حسابداری آدرس پوشه پشتیبان گیری اتوماتیک داده شده است
                    //باید فایل پشتیبان در این پوشه ایجاد گردد
                    path = CreateFileName(connectionString, path);
                    IsAutomatic = true;
                }

                DeleteTempFilesToken?.Cancel();
                token.ThrowIfCancellationRequested();

                //تهیه اولین نسخه بکاپ توسط سرویس اس کیو ال
                var CreateSqlBackuResult = await CreateSqlServerBackupFileAsync(path, connectionString);
                DatabaseName = CreateSqlBackuResult.value;
                res.AddReturnedValue(CreateSqlBackuResult);
                DeleteTempFilesToken?.Cancel();
                token.ThrowIfCancellationRequested();

                var PathForZipDirectory = Zip.Move2Temp(path);
                res.AddReturnedValue(PathForZipDirectory);
                DeleteTempFilesToken?.Cancel();
                token.ThrowIfCancellationRequested();

                try
                {
                    //please dont remove this try
                    //اگر به خطا خورد باید تابع ادامه پیدا کند و پشتیبان با پسوند .بک تهیه شود
                    res.AddReturnedValue(await CompressFile.CompressFileInstance.CompressFileAsync(PathForZipDirectory.value, path, token));
                }
                catch (Exception ex)
                {
                    WebErrorLog.ErrorInstence.StartErrorLog(ex);
                    res.AddReturnedValue(ex);
                }
                res.value = path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np").Replace(".np", ".npz2");

                //بنا به هر دلیلی امکان فشرده کردن فایل وجود نداشته است
                //باید همان بکاپ فشرده نشده به آدرس مقصد درخواستی مشتری متقل گردد
                if (res.HasError)
                {
                    var file = Directory.GetFiles(PathForZipDirectory.value).FirstOrDefault();
                    File.Move(file, path.ToLower().Replace(".npz2", ".np").Replace(".npz", ".np"));
                }

                OnCreateBackup?.Invoke(null,
                           new CreateBackupArgs() { Message = "", State = CreateBackupState.End });
            }
            catch (OperationCanceledException ex)
            {
                res.AddReturnedValue(ex);
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex, $"Error in Line {line}");
                res.AddReturnedValue(ex);
            }
            finally
            {
                DeleteTempFilesToken?.Cancel();
                DeleteTempFilesToken = new CancellationTokenSource();
                Task.Run(() => deleteTempsAsync(DeleteTempFilesToken.Token));
                Task.Run(() => BackupHistory.DeleteOldBackupsAsync());
                Task.Run(() => BackupHistory.SaveAsync(new BackupHistory()
                {
                    Date = DateTime.Now,
                    Guid = Guid ?? System.Guid.NewGuid(),
                    IsAutomatic = IsAutomatic,
                    DatabaseName = DatabaseName,
                    Path = path,
                    IsSuccess = !res.HasError,
                    Source = Source
                }));
            }
            return res;
        }

        private static async Task deleteTempsAsync(CancellationToken token)
        {
            try
            {
                await Task.Delay(60 * 1000, token);
                string tempDIR = Path.Combine(Application.StartupPath, "Temp");
                if (Directory.Exists(tempDIR))
                    Directory.Delete(tempDIR, true);
            }
            catch (OperationCanceledException) { }
            catch (ThreadAbortException) { }
            catch (Exception ex) { WebErrorLog.ErrorInstence.StartErrorLog(ex); }
        }

        private static async Task<ReturnedSaveFuncInfo> CreateSqlServerBackupFileAsync(string path, string connectionString)
        {
            var ret = new ReturnedSaveFuncInfo();
            string commandText = "";
            try
            {
                var inf = new FileInfo(path);
                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(inf.Name) || !Directory.Exists(inf.DirectoryName))
                {
                    ret.AddReturnedValue(ReturnedState.Error, $"آدرس {path} موجود یا معتبر نیست.");
                    return ret;
                }
                var cn = new SqlConnection(connectionString);
                ret.value = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
                var cmd = new SqlCommand($"Backup database [{ret.value }] to disk='{path}'", cn) { CommandType = CommandType.Text };
                commandText = cmd.CommandText;
                //تنها جای مجاز برای افزایش تایم اوت هست
                cmd.CommandTimeout = 5 * 60 * 1000;//5 minutes

                await cn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                cn.Close();
                commandText = "";
            }
            catch (SqlException ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ReturnedState.Error, $"خطا در تهیه نسخه پشتیبان اطلاعات \r\nحساب کاربری SQL دسترسی به مسیر نصب برنامه ندارد\r\n{ex.Message}//{ex?.InnerException?.Message ?? ""}");
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.AddReturnedValue(ex);
            }
            return ret;
        }

        public static async Task<ReturnedSaveFuncInfo> ReStoreDbAsync(string connectionString, string pathf, bool autoBackup, ENSource Source)
        {
            var ret = new ReturnedSaveFuncInfo();
            var cn = new SqlConnection();
            try
            {
                GC.Collect();
                SqlConnection.ClearAllPools();

                if (autoBackup)
                {

                    var dir = Application.StartupPath + "\\NovinPardazBackUp";
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    var file = Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly()?.Location)?.Replace(".exe", "__");
                    var d = Calendar.MiladiToShamsi(DateTime.Now).Replace("/", "_");
                    d += "__" + DateTime.Now.Hour + " - " + DateTime.Now.Minute;
                    file += d;

                    var filepath = dir + "\\" + file + ".Bak";
                    ret.AddReturnedValue(await BackupDbAsync(connectionString, Source, filepath, Guid.NewGuid()));
                }

                if (pathf.EndsWith(".NPZ") || pathf.EndsWith(".npz"))
                {
                    var zp = new Zip();
                    pathf = zp.ExtractTempDIR(pathf);
                }
                else if (pathf.EndsWith(".NPZ2") || pathf.EndsWith(".npz2"))
                {
                    try
                    {
                        var zp = new CompressFile();
                        var res = await zp.ExtractTempDIR(pathf);
                        ret.AddReturnedValue(res);
                        if (res.HasError) return ret;
                        pathf = res.value;
                    }
                    catch
                    {
                        var zp = new Zip();
                        pathf = zp.ExtractTempDIR(pathf);
                    }
                }

                var csb = new SqlConnectionStringBuilder(connectionString);
                var serverConnectionString =
                    connectionString.Replace("Initial Catalog=" + csb.InitialCatalog, "Initial Catalog=");
                cn = new SqlConnection(serverConnectionString) { ConnectionString = serverConnectionString };
                var cmd = new SqlCommand("RESTORE DATABASE [dbName] FROM  DISK = N'bkPath' WITH  FILE = 1,  MOVE N'novin' TO N'dbMDFPath',  MOVE N'novin_log' TO N'dbLDFPath',   NOUNLOAD,  REPLACE,  STATS = 5", cn);
                var dbInfo = new CLSServerDBs(connectionString);
                var backUpInfo = new DataBaseBackUpInfo(pathf, cn.ConnectionString);

                cmd.CommandText = cmd.CommandText.Replace("dbName", csb.InitialCatalog);
                cmd.CommandText = cmd.CommandText.Replace("bkPath", pathf);
                cmd.CommandText = cmd.CommandText.Replace("novin", backUpInfo.LogicalName);
                cmd.CommandText = cmd.CommandText.Replace("novin_log", backUpInfo.LogicalName + "_log.ldf");
                cmd.CommandText = cmd.CommandText.Replace("dbMDFPath", dbInfo.FileName);
                cmd.CommandText =
                    cmd.CommandText.Replace("dbLDFPath", dbInfo.FileName.ToLower().Replace(".mdf", "_log.Ldf"));

                //تایم اوت بالا بردن مجاز است
                cmd.CommandTimeout = 5 * 60 * 1000;//5 minutes

                cn.Open();
                var temp = cmd.CommandText;
                cmd.CommandText = "ALTER DATABASE [" + csb.InitialCatalog + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                cmd.ExecuteNonQuery();

                cmd.CommandText = temp;
                cmd.ExecuteNonQuery();

                // cmd.CommandText = NovinPardaz.Properties.Resources.DatabaseSettings;
                cmd.CommandText = cmd.CommandText.Replace("dbName", csb.InitialCatalog);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "ALTER DATABASE [" + csb.InitialCatalog + "] SET Multi_USER";
                cmd.ExecuteNonQuery();
                cn.Close();

                SqlConnection.ClearAllPools();
                Application.DoEvents();
            }
            catch (SqlException ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                try { cn.Close(); } catch { }
                ret.AddReturnedValue(ReturnedState.Error, $"حساب کاربری SQL دسترسی به مسیر نصب برنامه ندارد\r\n{ex.Message }//{ex?.InnerException?.Message ?? ""}");
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
                try { cn.Close(); } catch { }
                ret.AddReturnedValue(ex);
            }
            Task.Run(() => SetMultiUser(connectionString));
            return ret;
        }

        public static void SetMultiUser(string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(connectionString);
                string ServerConnectionString = connectionString.Replace("Initial Catalog=" + csb.InitialCatalog, "Initial Catalog=");
                SqlConnection cn = new SqlConnection(ServerConnectionString);
                SqlCommand cmd = new SqlCommand("ALTER DATABASE [" + csb.InitialCatalog + "] SET Multi_USER", cn);
                cmd.CommandType = CommandType.Text;

                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(ex);
            }
        }

        public static CreateDatabaseResult CreateDataBase(string dbName)
            => CreateDataBase(dbName, Settings.ServerConnectionsString);

        public static CreateDatabaseResult CreateDataBase(string dbName, string serverConnectionString)
        {
            var ret = new CreateDatabaseResult();
            if (string.IsNullOrEmpty(dbName))
            {
                ret.Status = EnCreateDataBase.DatabaseNameEmpty;
                ret.Result.AddReturnedValue(ReturnedState.Error, $"نام بانک اطلاعاتی خالی است.");
                return ret;
            }

            if (string.IsNullOrEmpty(serverConnectionString))
            {
                ret.Status = EnCreateDataBase.ServerConnectionStringError;
                ret.Result.AddReturnedValue(ReturnedState.Error, $"رشته اتصال به سرور خالی است.");
                return ret;
            }

            try
            {
                var cbs = new SqlConnectionStringBuilder(serverConnectionString) { InitialCatalog = "master" };

                if (IsExistDatabase(dbName, cbs.ConnectionString))
                {
                    ret.Result.AddReturnedValue(ReturnedState.Error, $"بانک اطلاعاتی با نام {dbName} \r\nدر سرویس {serverConnectionString}\r\n در حال حاضر وجود دارد.");
                    ret.Status = EnCreateDataBase.DatabaseExists;
                    return ret;
                }

                var cn = new SqlConnection(cbs.ConnectionString);
                var cmd = new SqlCommand("", cn) { CommandType = CommandType.Text };
                var dbPath = Application.StartupPath + "\\DataBase";
                if (Settings.SetDistanationFolder(cn.ConnectionString))
                {
                    cmd.CommandText = "CREATE DATABASE [DBName] ON  PRIMARY ( NAME = N'DBName'" +
                                  ",FILENAME = N'DBPath\\DBName.mdf', SIZE = 10240KB , FILEGROWTH = 10240KB )" +
                                  "LOG ON ( NAME = N'DBName_log',FILENAME = N'DBPath\\DBName_log.ldf'" +
                                  ",SIZE=10240KB,FILEGROWTH=10%)";
                    cmd.CommandText = cmd.CommandText.Replace("DBName", dbName);
                    cmd.CommandText = cmd.CommandText.Replace("DBPath", dbPath);
                }
                else
                {
                    cmd.CommandText = "CREATE DATABASE [DBName]";
                    cmd.CommandText = cmd.CommandText.Replace("DBName", dbName);
                }

                var constr = cn.ConnectionString.Replace("master", dbName);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();

                //return cn.ConnectionString.Replace("master", DbName);
                ret.ConnectionString = constr;
                ret.Status = EnCreateDataBase.Success;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("because it already exists".ToLower()))
                    ret.Status = EnCreateDataBase.DatabaseExists;
                else
                    WebErrorLog.ErrorInstence.StartErrorLog(ex);
                ret.Result.AddReturnedValue(ex);
            }
            return ret;
        }

        public static bool IsExistDatabase(string dbName, string connectionString)
        {
            try
            {
                var cn = new SqlConnection(connectionString);
                var cmd = new SqlCommand("SELECT case when exists (select 1 from sys.Databases where Name = @DbName) then 1 else 0 end as DbExists", cn);
                cmd.Parameters.AddWithValue("@DbName", dbName);
                cn.Open();
                var exist = int.Parse(cmd.ExecuteScalar().ToString());
                cn.Close();
                return exist != 0;
            }
            catch (Exception e)
            {
                WebErrorLog.ErrorInstence.StartErrorLog(e);
                return false;
            }
        }
    }
}
